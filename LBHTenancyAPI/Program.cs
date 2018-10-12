using System;

namespace LBHTenancyAPI
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(server=> server.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30))
                .Build();
    }
}
