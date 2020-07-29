using System.Security.Cryptography.X509Certificates;
using IdentityModel;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(".", LogEventLevel.Error)
                .WriteTo.Console()
                .CreateLogger();

            CreateWebHostBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.AddSerilog(Log.Logger);
                })
                //.UseKestrel(options =>
                //{
                //    options.ListenLocalhost(5000, listenOptions => listenOptions.UseHttps(adapterOptions =>
                //    {
                //        adapterOptions.ServerCertificate = Startup.GetIdServerCertificate();
                //    }));
                //})
                .UseStartup<Startup>()
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
        ;

    }
}
