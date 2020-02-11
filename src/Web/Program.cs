using System;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Codidact.Authentication.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Codidact.Authentication.Web";

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConsole();
                })
                .UseStartup<Startup>();
        }
    }
}
