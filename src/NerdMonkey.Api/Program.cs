using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NerdMonkey.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://localhost:5100");
                    webBuilder.UseStartup<Startup>();
                }).UseDefaultServiceProvider((context, options) => {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        options.ValidateOnBuild = true;
                        options.ValidateScopes = true;
                    }
                });
    }
}
