using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;


        public Startup(IConfiguration conf, IHostingEnvironment env)
        {
            _configuration = conf;
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsDevelopment())
            {
                var dllPath = _configuration["StObjMap:Path"];
                if (dllPath != null)
                {
                    var parentPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppContext.BaseDirectory)))));
                    dllPath = Path.Combine(parentPath, dllPath);
                    File.Copy(dllPath, Path.Combine(AppContext.BaseDirectory, "CK.StObj.AutoAssembly.dll"), true);
                }
            }
            services.AddStObjMap("CK.StObj.AutoAssembly");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
