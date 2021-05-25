using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Supplyer.API.Configuration
{
    public static class ConfigHelper
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services, SupplyApiConfiguration supplyApiConfiguration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(supplyApiConfiguration.ApiVersion, new OpenApiInfo { Title = supplyApiConfiguration.ApiName, Version = supplyApiConfiguration.ApiVersion });
                #region 读取xml信息

                // 使用反射获取xml文件。并构造出文件的路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                c.IncludeXmlComments(xmlPath, true);
                #endregion

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{supplyApiConfiguration.IdentityServerBaseUrl}/connect/authorize"),
                            TokenUrl = new Uri($"{supplyApiConfiguration.IdentityServerBaseUrl}/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { supplyApiConfiguration.OidcApiName, supplyApiConfiguration.ApiName }
                            }
                        }
                    }
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            return services;
        }

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            var identityUrl = configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                //options.Audience = "supply";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });
            return services;
        }

        public static IServiceCollection AddAuthMVCInteractiveConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetValue<string>("IdentityUrl");
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = identityUrl;
                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.SaveTokens = true;
            });
            return services;
        }

        public static IServiceCollection AddAuthJwtConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetValue<string>("IdentityUrl");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // base-address of your identityserver
                    options.Authority = identityUrl;

                    // if you are using API resources, you can specify the name here
                    options.Audience = "resource1";

                    // IdentityServer emits a typ header by default, recommended extra check
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });
            return services;
        }
        public static IServiceCollection AddAuthJwtRefConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetValue<string>("IdentityUrl");
            services.AddAuthentication("token")
                // JWT tokens
                .AddJwtBearer("token", options =>
                {
                    // base-address of your identityserver
                    options.Authority = identityUrl;

                    // if you are using API resources, you can specify the name here
                    options.Audience = "resource1";

                    // IdentityServer emits a typ header by default, recommended extra check
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

                    // if token does not contain a dot, it is a reference token
                    options.ForwardDefaultSelector = context => "introspection";
                })

                // reference tokens
                .AddOAuth2Introspection("introspection", options =>
                {
                    options.Authority = identityUrl;
                    options.ClientId = "supply";
                    options.ClientSecret = "secret";
                });
            return services;
        }

    }
}
