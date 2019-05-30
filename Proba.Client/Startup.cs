using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Proba
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Fubar>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }

    public class Fubar
    {
        public IEnumerable<(string, int, DateTime, decimal)> Data { get; set; }

        public Fubar()
        {
            Data = new List<(string, int, DateTime, decimal)>
            {
                ("Hey", 12, DateTime.Now, 124.01m),
                ("Hello", 45, DateTime.Now, 124.01m),
                ("Hi", 7, DateTime.Now.AddMinutes(123), -14.31m),
                ("Howdy", 12, DateTime.MinValue, 0.456m)
            };
        }
    }
}
