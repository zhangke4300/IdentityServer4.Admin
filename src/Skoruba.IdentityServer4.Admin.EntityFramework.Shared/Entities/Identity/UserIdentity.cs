using Microsoft.AspNetCore.Identity;
using System;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity
{
	public class UserIdentity : IdentityUser
	{
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