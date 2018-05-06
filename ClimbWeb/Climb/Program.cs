using System;
using Climb.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Climb
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch(Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        private static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();
    }
}