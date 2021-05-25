using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supplyer.API.Configuration
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly SupplyApiConfiguration _supplyApiConfiguration;

        public AuthorizeCheckOperationFilter(SupplyApiConfiguration supplyApiConfiguration)
        {
            _supplyApiConfiguration = supplyApiConfiguration;
        }
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize =
              context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
              || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!hasAuthorize) return;

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "无访问权限" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "禁止访问" });

            operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecurityScheme 
                            {
                                Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme,Id = "oauth2"}
                            }
                        ] = new[] { _supplyApiConfiguration.OidcApiName }
                    }
                };
        }
    }

}
