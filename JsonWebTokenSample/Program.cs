using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace JsonWebTokenSample
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // This is a different config builder than the one in Startup.
            var configHosting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            var hostBuilder = new WebHostBuilder()
                .UseConfiguration(configHosting)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();


            var host = hostBuilder.Build();

            host.Run();
        }
    }
}

