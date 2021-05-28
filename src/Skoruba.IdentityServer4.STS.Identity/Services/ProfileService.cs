using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.DbContexts;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.STS.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<UserIdentity> _claimsFactory;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly AdminIdentityDbContext _adminIdentityDbContext;

        public ProfileService(UserManager<UserIdentity> userManager
            , AdminIdentityDbContext adminIdentityDbContext
            , IUserClaimsPrincipalFactory<UserIdentity> claimsFactory)
        {
            _userManager = userManager;
            _adminIdentityDbContext = adminIdentityDbContext;
            _claimsFactory = claimsFactory;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            var roleClaims = await getClaimValues(user);
            if (roleClaims.Count() > 0)
            {
                var permissions = roleClaims?.Aggregate("", (s, permission) => s + permission);
                // Add custom claims in token here based on user properties or any other source
                claims.Add(new Claim("pemissons", permissions ?? string.Empty));
            }
            if (claims.Where(c => c.Type == "超级管理员").Count() > 0)
                claims.Add(new Claim("datakey", "|"));
            else
                claims.Add(new Claim("datakey", user.DataKey ?? string.Empty));
            context.IssuedClaims = claims;
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<string>> getClaimValues(UserIdentity user)
        {
            var queryRoleClaims = from ur in _adminIdentityDbContext.UserRoles where ur.UserId == user.Id
                                  join rc in _adminIdentityDbContext.RoleClaims on ur.RoleId equals rc.RoleId
                                  select rc.ClaimValue;
            return await Task.FromResult(queryRoleClaims.ToList().Distinct());
        }

        private ushort TryParse(string claimValue)
        {
            return ushort.TryParse(claimValue, out ushort cr) ? cr : (ushort)0;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
