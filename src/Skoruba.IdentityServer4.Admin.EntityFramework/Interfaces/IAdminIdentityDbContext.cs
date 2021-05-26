﻿using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces
{
    public interface IAdminIdentityDbContext
    {
        public DbSet<HierarchyBase> HierarchyBases { get; set; }
    }
}
