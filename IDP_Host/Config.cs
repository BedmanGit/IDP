﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using static IdentityModel.OidcConstants;
using GrantTypes = IdentityServer4.Models.GrantTypes;

namespace IDP_Host
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var customProfile = new IdentityResource(
            name: "personal_infor",
            displayName: "Custom profile",
            claimTypes: new[] { "dob", "city", "country", "interests","lookingfor", "knownas", "introduction" });

            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource("roles", "You role(s)", new List<string>(){"role"}),
                customProfile
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("DatingApp-API", "Dating App API",
                new List<string>{"role", "dob", "city", "country", "interests","lookingfor", "knownas", "introduction"})
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "Client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("thisismysecret".Sha256()) },

                    AllowedScopes = { "MyAPI" }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "MyWeb",
                    ClientName = "MVC_Client",
                    ClientUri = "https://localhost:44380/",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("mysecret".Sha256()) },
                   
                    RedirectUris = { "https://localhost:44380/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44380/signout-oidc",

                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Address,
                        "roles",
                        "personal_infor",
                        "DatingApp-API"
                    },
                    PostLogoutRedirectUris = {
                        "https://localhost:44380/signout-callback-oidc"
                    },
                },

                // SPA client using implicit flow
                new Client
                {
                    ClientId = "DatingApp-SPA",
                    ClientName = "SPA Client",
                    ClientUri = "http://localhost:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =
                    {
                        "http://localhost:4200/assets/oidc-login-redirect.html",
                    },
                 
                    PostLogoutRedirectUris = { "http://localhost:4200/signout-callback-oidc" },
                    AllowedCorsOrigins = { "http://localhost:4200/" },

                    AllowedScopes = { "openid", "profile", "personal_infor", "DatingApp-API" }
                }
            };
        }
    }
}