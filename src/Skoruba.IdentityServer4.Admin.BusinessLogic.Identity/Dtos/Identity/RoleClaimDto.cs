using System.ComponentModel.DataAnnotations;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Base;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Validate;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class RoleClaimDto<TKey> : BaseRoleClaimDto<TKey>, IRoleClaimDto
    {
        [Required]
        public string ClaimType { get; set; }


        [Required]
        [StringLength(2, MinimumLength = 2,ErrorMessage ="长度必须是两位")]
        public string ClaimValue { get; set; }
    }
}
