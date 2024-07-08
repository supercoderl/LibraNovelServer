using Asp.Versioning;
using LibraNovel.Application;
using LibraNovel.Application.ViewModels.Email;
using LibraNovel.Infrastructure.Data;
using LibraNovel.Infrastructure.Data.Context;
using LibraNovel.Infrastructure.IoC;
using LibraNovel.WebAPI.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;


namespace LibraNovel.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // Initialize logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config).WriteTo
                .File("Logs/log.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp} {Message}{NewLine:1}{Exception:1}").WriteTo
                .File(new JsonFormatter(), "Logs/log.json", rollingInterval: RollingInterval.Day).WriteTo
                .Console()
                .CreateLogger();

            var app = builder.Build();

            try
            {
                Log.Information("Application is starting");

                SeedDatabase(app);


                app.Run();

            }
            catch (Exception e)
            {
                Log.Error(e, "The application failed to start!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            })
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            }).ConfigureServices((hostContext, services) =>
            {
                // Add services to the container.
                services.AddIoCService(hostContext.Configuration);
                services.AddSwaggerExtension();
                services.AddApiVersioningExtension();
                services.AddIdentityInfrastructure(hostContext.Configuration);
                services.AddApplicationLayer();
            });

        private static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var libraNovelContext = services.GetRequiredService<LibraNovelContext>();
                    LibraNovelContextSeed.SeedAsync(libraNovelContext, loggerFactory).Wait();
                }
                catch (Exception exception)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(exception, "An error occurred seeding the DB.");
                }
            }
        }
    }
}
