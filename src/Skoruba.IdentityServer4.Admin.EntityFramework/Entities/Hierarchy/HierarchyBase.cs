// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Entities
{

    /// <summary>
    /// This contains the class that all the hierarchical tenant classes inherit from
    /// NOTE: The DataKey is created by the business logic when a new entry is created, or updated if moved
    /// </summary>
    public class HierarchyBase : IDataKey
    {
        public int Id { get; set; }       

        //描述  
        public string Text { get; set; }

        //描述
        public string Description { get; set; }

        //子级到父级的ID
        public string DataKey { get; set; }
        [JsonIgnore]
        //父级ID
        public int? Pid { get; set; }
        [ForeignKey("Pid")]
        [JsonIgnore]
        public virtual HierarchyBase Parent { get; set; }
        public virtual ICollection<HierarchyBase> Children { get; set; }
    }
}