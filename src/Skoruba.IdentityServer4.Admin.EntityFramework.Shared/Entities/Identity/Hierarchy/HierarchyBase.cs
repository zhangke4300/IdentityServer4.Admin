// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.DbContexts;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity
{

    /// <summary>
    /// This contains the class that all the hierarchical tenant classes inherit from
    /// NOTE: The DataKey is created by the business logic when a new entry is created, or updated if moved
    /// </summary>
    public class HierarchyBase : IDataKey
    {
        private HashSet<HierarchyBase> _children;

        protected HierarchyBase(string name) // needed by EF Core
        {
            Name = name;
        } 

        protected HierarchyBase(string name, HierarchyBase parent)
        {
            Name = name;
            Parent = parent;
            _children = new HashSet<HierarchyBase>();  //Used when creating a new version - not used by EF Core
        }

        protected static void AddTenantToDatabaseWithSaveChanges(HierarchyBase newTenant, AdminIdentityDbContext context)
        {
            if (newTenant == null) throw new ArgumentNullException(nameof(newTenant));

            if (!(newTenant is ItemFirst))
            {
                if (newTenant.Parent == null)
                    throw new ApplicationException($"The parent cannot be null in type {newTenant.GetType().Name}.");
                if (newTenant.Parent.ParentId == 0)
                    throw new ApplicationException($"The parent {newTenant.Parent.Name} must be already in the database.");             
            }
            if (context.Entry(newTenant).State != EntityState.Detached)
                throw new ApplicationException($"You can't use this method to add a tenant that is already in the database.");

            //We have to do this request using a transaction to make sure the DataKey is set properly
            using (var transaction = context.Database.BeginTransaction())
            {
                //set up the backward link (if Parent isn't null)
                newTenant.Parent?._children.Add(newTenant);
                context.Add(newTenant);  //also need to add it in case its the company
                // Add this to get primary key set
                context.SaveChanges();

                //Now we can set the DataKey
                newTenant.SetDataKeyFromHierarchy();
                context.SaveChanges();

                transaction.Commit();
            }
        }

        /// <summary>
        /// Each tenant has its own primary key (as well as the AccessKey used for multi-tenant filtering)
        /// </summary>
        [Key]
        public int DataId { get; private set; }

        /// <summary>
        /// This holds the DataKey, which is hierarchical in nature, contains a string that reflects the
        /// position of the tenant in the hierarchy. I do this by building a string which contains the PK
        /// i.e. it has the PK of each parent as hex strings, with a | as a separator and a * at the end.
        /// e.g. 1|2|F*
        /// </summary>
        public string DataKey { get; private set; }

        /// <summary>
        /// This is the name of the tenant: could be CompanyName, Area/SubGroup or retail outlet name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// This is the foreign key that points to its parent - null if if its the Company tenant class
        /// </summary>
        public int? ParentId { get; private set; }

        /// <summary>
        /// This is a pointer to its parent - it will be null if its the Company tenant class
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public HierarchyBase Parent { get; private set; }

        /// <summary>
        /// This holds the tenants one level below 
        /// </summary>
        public IEnumerable<HierarchyBase> Children => _children?.ToList();

        public override string ToString()
        {
            return $"{GetType().Name}: Name = {Name}, DataKey = {DataKey ?? "<null>"}";
        }

        public int ExtractCompanyId()
        {
            if (DataKey == null)
                throw new NullReferenceException("The DataKey must be set before we can extract the CompanyId.");

            return int.Parse(DataKey.Substring(0, DataKey.IndexOf('|')), NumberStyles.HexNumber);
        }

        //----------------------------------------------------
        // public methods

        public void MoveToNewParent(HierarchyBase newParent, DbContext context)
        {
            void SetKeyExistingHierarchy(HierarchyBase existingTenant)
            {
                existingTenant.SetDataKeyFromHierarchy();
                if (existingTenant.Children == null)
                    context.Entry(existingTenant).Collection(x => x.Children).Load();

                if (!existingTenant._children.Any())
                    return;
                foreach (var tenant in existingTenant._children)
                {                   
                    SetKeyExistingHierarchy(tenant);
                }
            }

            if (this is ItemFirst)
                throw new ApplicationException($"你不能移动根数据.");
            if (newParent == null)
                throw new ApplicationException($"父级不能为空.");
            if (newParent.ParentId == 0)
                throw new ApplicationException($"父级不存在.");
            if (newParent == this)
                throw new ApplicationException($"父级不是自己.");
            if (context.Entry(this).State == EntityState.Detached)
                throw new ApplicationException($"此方法仅仅改变父级.");
            if (context.Entry(newParent).State == EntityState.Detached)
                throw new ApplicationException($"父级没查找到.");

            Parent._children?.Remove(this);
            Parent = newParent;
            //Now change the data key for all the hierarchy from this entry down
            SetKeyExistingHierarchy(this);
        }

        public void MoveToNewParent(int parentId, DbContext context)
        {
            var parent = context.Find<HierarchyBase>(parentId);
            MoveToNewParent(parent, context);
        }

        //---------------------------------
        //private methods

        /// <summary>
        /// This sets the DataKey to create the hierarchical DataAccess key.
        /// See <see cref="DataKey"/> for more on the format of the hierarchical DataAccess key.
        /// </summary>
        private void SetDataKeyFromHierarchy()
        {
            if (!(this is ItemFirst) && Parent == null)
                throw new ApplicationException($"The parent cannot be null if this tenant isn't a {nameof(ItemFirst)}.");
            if (DataId == 0)
                throw new ApplicationException("This class must have a primary key set before calling this method.");

            DataKey = $"{DataId:x}|";
            if (Parent != null)
            {
                if (Parent.DataId == 0)
                    throw new ApplicationException("The parent class must have a primary key set before calling this method.");
                DataKey = Parent.DataKey + DataKey;
            }
        }


    }
}