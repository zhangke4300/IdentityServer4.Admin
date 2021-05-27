using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class HierarchyDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string DataKey { get; set; }

        public ICollection<HierarchyDto> Children { get; set; }

        //public int Id { get; set; }

        ////描述  
        //public string Text { get; set; }

        ////描述
        //public string Description { get; set; }

        ////子级到父级的ID
        //public string DataKey { get; set; }
        ////父级ID
        //public int? Pid { get; set; }

        //public virtual HierarchyDto Parent { get; set; }
        //public virtual ICollection<HierarchyDto> Children { get; set; }
    }
}
