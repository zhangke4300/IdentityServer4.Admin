namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces
{
    public interface IRoleClaimDto : IBaseRoleClaimDto
    {
        string ClaimType { get; set; }
        ushort ClaimValue { get; set; }
    }
}
