using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Validate
{
    public class ClaimValueCheckAttribute: ValidationAttribute
    {
        private readonly int _baseNum;
        public ClaimValueCheckAttribute(int baseNum)
        {
            _baseNum = baseNum;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var parseResult = int.TryParse(value.ToString(), out int intValue);
            if (!parseResult || intValue % _baseNum == 0)
            {
                // here we are verifying whether the 2 values are equal
                // but you could do any custom validation you like
                return new ValidationResult($"被{_baseNum}整除的数为系统保留值！");
            }
            return null;
        }
    }
}
