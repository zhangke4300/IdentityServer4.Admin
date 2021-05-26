
namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity
{
    /// <summary>
    /// This is the root entry for a multi-tenant entry. There should be only one Company per multi-tenant
    /// </summary>
    public class ItemFirst : HierarchyBase
    {
        private ItemFirst(string name) : base(name) {  } 
    }
}