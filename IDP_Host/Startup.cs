﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4;
using IDP.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using IDP.Entities;
using IDP.Services;
using IDP;
using System.Security.Cryptography.X509Certificates;

namespace IDP_Host
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                options.AuthenticationDisplayName = "Windows";
            });

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddUserStore();
            //.AddTestUsers(TestUsers.Users);

            // in-memory, json config
            builder.AddInMemoryIdentityResources(Config.GetIdentityResources()); //Configuration.GetSection("IdentityResources")
            builder.AddInMemoryApiResources(Config.GetApis()); //Configuration.GetSection("ApiResources")
            builder.AddInMemoryClients(Config.GetClients()); //Configuration.GetSection("clients")

            if (Environment.IsDevelopment())
            {
                 //builder.AddDeveloperSigningCredential();
                builder.AddSigningCredential(LoadCertificateFromStore());
            }
            else
            {
                //throw new Exception("need to configure key material");
                builder.AddSigningCredential(LoadCertificateFromStore());
            }

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
        }

        public void Configure(IApplicationBuilder app, UserContext userContext)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //userContext.Database.Migrate();
            userContext.EnsureSeedDataForContext();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        public X509Certificate2 LoadCertificateFromStore()
        {
            //string thumbPrint = "7835fd8ce81d30cad7497c1890d7add8e4f0a1d8".ToUpper();
            string thumbPrint = "76aa7e2e199c7d1bda3d1639567752fb2cb8eb12".ToUpper();


            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, true);
                if(certCollection.Count == 0)
                {
                    throw new Exception("Certificate not found.");
                }
                return certCollection[0];

                
            }
        }

    }
}