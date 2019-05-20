using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyWeb
{
    public static class DI
    {
        public static void AddDependencyInjectionClass(IServiceCollection services)
        {
            services.AddScoped<HttpClient>();
        }
    }
}
