using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CacheSample {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDistributedRedisCache(options => {
                options.Configuration = "localhost";
                options.InstanceName = "local";
            });

            services.AddHttpClient(Constants.DATA_CLIENT, options => {
                options.BaseAddress = new Uri("https://localhost:33333");
                options.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<ICacheService, CacheService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
