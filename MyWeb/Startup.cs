using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddScoped<HttpClient>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "MyCookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("MyCookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:44316";
                    options.RequireHttpsMetadata = true;
                    options.ClientId = "MyWeb";
                    options.SaveTokens = true;
                    options.ClientSecret = "mysecret";
                    options.ResponseType = "code id_token";
                    options.Scope.Add("profile");
                    options.Scope.Add("openid");
                    options.Scope.Add("address");
                    options.Scope.Add("roles");
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Events = new OpenIdConnectEvents()
                    {
                        OnTokenValidated = tokenValidatedContext =>
                        {
                            var identity = tokenValidatedContext.Principal.Identity as ClaimsIdentity;
                            var subjectClaim = identity.Claims.FirstOrDefault(z => z.Type == "sub");

                            var newClaimsIdentity = new ClaimsIdentity(tokenValidatedContext.Scheme.Name,
                                "given_name", 
                                "role");
                            newClaimsIdentity.AddClaim(subjectClaim);
                            tokenValidatedContext.Principal = new ClaimsPrincipal(newClaimsIdentity);
                            //tokenValidatedContext.Success();
                            return Task.FromResult(0);
                        },
                        OnUserInformationReceived = userInformationReceivedContext =>
                        {
                            userInformationReceivedContext.User.Remove("address");
                            return Task.FromResult(0);
                        }
                    };
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //  app.UseCookiePolicy();
            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
