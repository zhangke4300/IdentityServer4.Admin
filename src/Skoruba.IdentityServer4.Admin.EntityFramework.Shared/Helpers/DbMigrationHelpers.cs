using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.IdentityServer4.Admin.EntityFramework.Configuration.Configuration;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Helpers
{
	public static class DbMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use these steps bellow:
        /// https://github.com/skoruba/IdentityServer4.Admin#ef-core--data-access
        /// </summary>
        /// <param name="host"></param>
        /// <param name="applyDbMigrationWithDataSeedFromProgramArguments"></param>
        /// <param name="seedConfiguration"></param>
        /// <param name="databaseMigrationsConfiguration"></param>
        public static async Task ApplyDbMigrationsWithDataSeedAsync<TIdentityServerDbContext, TIdentityDbContext,
            TPersistedGrantDbContext, TLogDbContext, TAuditLogDbContext, TDataProtectionDbContext, TUser, TRole>(
            IHost host, bool applyDbMigrationWithDataSeedFromProgramArguments, SeedConfiguration seedConfiguration,
            DatabaseMigrationsConfiguration databaseMigrationsConfiguration)
            where TIdentityServerDbContext : DbContext, IAdminConfigurationDbContext
            where TIdentityDbContext : DbContext, IAdminIdentityDbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TLogDbContext : DbContext, IAdminLogDbContext
            where TAuditLogDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
            where TDataProtectionDbContext : DbContext, IDataProtectionKeyContext
            where TUser : IdentityUser, new()
            where TRole : IdentityRole, new()
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                if ((databaseMigrationsConfiguration != null && databaseMigrationsConfiguration.ApplyDatabaseMigrations)
                    || (applyDbMigrationWithDataSeedFromProgramArguments))
                {
                    await EnsureDatabasesMigratedAsync<TIdentityDbContext, TIdentityServerDbContext, TPersistedGrantDbContext, TLogDbContext, TAuditLogDbContext, TDataProtectionDbContext>(services);
                }

                if ((seedConfiguration != null && seedConfiguration.ApplySeed)
                    || (applyDbMigrationWithDataSeedFromProgramArguments))
                {
                    await EnsureSeedDataAsync<TIdentityDbContext,TIdentityServerDbContext, TUser, TRole>(services);
                }
            }
        }

        public static async Task EnsureDatabasesMigratedAsync<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TLogDbContext, TAuditLogDbContext, TDataProtectionDbContext>(IServiceProvider services)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext
            where TConfigurationDbContext : DbContext
            where TLogDbContext : DbContext
            where TAuditLogDbContext : DbContext
            where TDataProtectionDbContext : DbContext
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<TPersistedGrantDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TIdentityDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TConfigurationDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TLogDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TAuditLogDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TDataProtectionDbContext>())
                {
                    await context.Database.MigrateAsync();
                }
            }
        }

        public static async Task EnsureSeedDataAsync<TIdentityDbContext,TIdentityServerDbContext, TUser, TRole>(IServiceProvider serviceProvider)
        where TIdentityDbContext : DbContext, IAdminIdentityDbContext
        where TIdentityServerDbContext : DbContext, IAdminConfigurationDbContext
        where TUser : IdentityUser, new()
        where TRole : IdentityRole, new()
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TIdentityServerDbContext>();
                var contextIdentity = scope.ServiceProvider.GetRequiredService<TIdentityDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<TRole>>();
                var idsDataConfiguration = scope.ServiceProvider.GetRequiredService<IdentityServerData>();
                var idDataConfiguration = scope.ServiceProvider.GetRequiredService<IdentityData>();
                var hdDataConfiguration = scope.ServiceProvider.GetRequiredService<HierarchyData>();

                await EnsureSeedIdentityServerData(context, idsDataConfiguration);
                await EnsureSeedHierarchyData(contextIdentity, hdDataConfiguration);
                await EnsureSeedIdentityData(userManager, roleManager, idDataConfiguration);
            }
        }

        /// <summary>
        /// Generate default admin user / role
        /// </summary>
        private static async Task EnsureSeedIdentityData<TUser, TRole>(UserManager<TUser> userManager,
            RoleManager<TRole> roleManager, IdentityData identityDataConfiguration)
            where TUser : IdentityUser, new()
            where TRole : IdentityRole, new()
        {
            // adding roles from seed
            foreach (var r in identityDataConfiguration.Roles)
            {
                if (!await roleManager.RoleExistsAsync(r.Name))
                {
                    var role = new TRole
                    {
                        Name = r.Name
                    };

                    var result = await roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        foreach (var claim in r.Claims)
                        {
                            await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(claim.Type, claim.Value));
                        }
                    }
                }
            }

            // adding users from seed
            foreach (var user in identityDataConfiguration.Users)
            {
                var identityUser = new TUser
                {
                    UserName = user.Username,
                    Email = user.Email,
                    EmailConfirmed = true
                };

                var userByUserName = await userManager.FindByNameAsync(user.Username);
                var userByEmail = await userManager.FindByEmailAsync(user.Email);

                // User is already exists in database
                if (userByUserName != default || userByEmail != default)
                {
                    continue;
                }

                // if there is no password we create user without password
                // user can reset password later, because accounts have EmailConfirmed set to true
                var result = !string.IsNullOrEmpty(user.Password)
                ? await userManager.CreateAsync(identityUser, user.Password)
                : await userManager.CreateAsync(identityUser);

                if (result.Succeeded)
                {
                    foreach (var claim in user.Claims)
                    {
                        await userManager.AddClaimAsync(identityUser, new System.Security.Claims.Claim(claim.Type, claim.Value));
                    }

                    foreach (var role in user.Roles)
                    {
                        await userManager.AddToRoleAsync(identityUser, role);
                    }
                }
            }
        }


        private static async Task EnsureSeedHierarchyData<TIdentityDbContext>(TIdentityDbContext context, HierarchyData hierarchyData)
            where TIdentityDbContext : DbContext, IAdminIdentityDbContext
        {
            if (context.HierarchyBases.Count() > 0)
                return;
            string getKeyId(HierarchyBase hierarchy)
            {
                if (hierarchy == null)
                    return (context.HierarchyBases.Where(o => !o.Pid.HasValue).Count() + 1).ToString();
                else
                    return (context.HierarchyBases.Where(o => o.Pid == hierarchy.Id).Select(o => o.Text).Distinct().Count() + 1).ToString();
            }
            HierarchyBase getHierarchyDataModel(string name, HierarchyBase hierarchy)
            {
                if (hierarchy != null)
                {
                    if (name.Contains(","))
                    {
                        foreach (string item in name.Split(","))
                        {
                            var exist = context.HierarchyBases.Where(o => o.Parent.Id == hierarchy.Id && o.Text == item.Trim()).FirstOrDefault();
                            if (exist != null) continue;
                            var entity = context.HierarchyBases.Add(new HierarchyBase()
                            {
                                Text = item.Trim(),
                                Pid = hierarchy.Id,
                                Description = item.Trim(),
                                DataKey = $"{hierarchy.DataKey}{getKeyId(hierarchy)}|"
                            }).Entity;
                            context.SaveChanges();
                        }
                        //包含<,>，表示有多个平级，默认为末级，返回null，跳出循环
                        return null;
                    }
                    else
                    {
                        var exist = context.HierarchyBases.Where(o => o.Parent.Id == hierarchy.Id && o.Text == name.Trim()).FirstOrDefault();
                        if (exist != null) return exist;
                        var entity = context.HierarchyBases.Add(new HierarchyBase()
                        {
                            Text = name.Trim(),
                            Pid = hierarchy.Id,
                            Description = name.Trim(),
                            DataKey = $"{hierarchy.DataKey}{getKeyId(hierarchy)}|"
                        }).Entity;
                        context.SaveChanges();
                        return entity;
                    }                    
                }
                else
                {
                    var exist = context.HierarchyBases.Where(o => !o.Pid.HasValue && o.Text == name).FirstOrDefault();
                    if (exist != null) return exist;
                    var entity = context.HierarchyBases.Add(new HierarchyBase()
                    {
                        Text = name.Trim(),
                        Description = name.Trim(),
                        DataKey = $"|{getKeyId(null)}|"
                    }).Entity;
                    context.SaveChanges();
                    return entity;
                }
            }
            foreach (var branch in hierarchyData.Branchs)
            {
                var hierarchNames = branch.Split('|');
                HierarchyBase parent = null;
                foreach(var name in hierarchNames)
                {
                    parent = getHierarchyDataModel(name, parent);
                    if (parent == null) break;
                }
                
            }
            await Task.CompletedTask;
        }


        /// <summary>
        /// Generate default clients, identity and api resources
        /// </summary>
        private static async Task EnsureSeedIdentityServerData<TIdentityServerDbContext>(TIdentityServerDbContext context, IdentityServerData identityServerDataConfiguration)
            where TIdentityServerDbContext : DbContext, IAdminConfigurationDbContext
        {
            foreach (var resource in identityServerDataConfiguration.IdentityResources)
            {
                var exits = await context.IdentityResources.AnyAsync(a => a.Name == resource.Name);

                if (exits)
                {
                    continue;
                }

                await context.IdentityResources.AddAsync(resource.ToEntity());
            }

            foreach (var apiScope in identityServerDataConfiguration.ApiScopes)
            {
                var exits = await context.ApiScopes.AnyAsync(a => a.Name == apiScope.Name);

                if (exits)
                {
                    continue;
                }

                await context.ApiScopes.AddAsync(apiScope.ToEntity());
            }

            foreach (var resource in identityServerDataConfiguration.ApiResources)
            {
                var exits = await context.ApiResources.AnyAsync(a => a.Name == resource.Name);

                if (exits)
                {
                    continue;
                }

                foreach (var s in resource.ApiSecrets)
                {
                    s.Value = s.Value.ToSha256();
                }

                await context.ApiResources.AddAsync(resource.ToEntity());
            }


            foreach (var client in identityServerDataConfiguration.Clients)
            {
                var exits = await context.Clients.AnyAsync(a => a.ClientId == client.ClientId);

                if (exits)
                {
                    continue;
                }

                foreach (var secret in client.ClientSecrets)
                {
                    secret.Value = secret.Value.ToSha256();
                }

                client.Claims = client.ClientClaims
                    .Select(c => new ClientClaim(c.Type, c.Value))
                    .ToList();

                await context.Clients.AddAsync(client.ToEntity());
            }

            await context.SaveChangesAsync();
        }
    }
}
