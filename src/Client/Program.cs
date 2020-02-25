using System;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Codidact.Authentication.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Codidact.Authentication.Client";

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
                .UseEnvironment(Environments.Development)
                .UseStartup<Startup>();
        }
    }
}
