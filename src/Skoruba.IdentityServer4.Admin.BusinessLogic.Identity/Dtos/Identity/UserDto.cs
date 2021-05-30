using System;
using System.ComponentModel.DataAnnotations;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Base;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class UserDto<TKey> : BaseUserDto<TKey>, IUserDto
    {        
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_@\-\.\+]+$")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
        public string DataKey { get; set; }

        /// <summary>
        /// 默认地址ID 
        /// </summary>
        public long? AddrId { get; set; }
        public string LastIpAddress { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
