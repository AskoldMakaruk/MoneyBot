using BotFramework.Extensions.Hosting;
using BotFramework.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyBot.DB;
using MoneyBot.DB.Model;
using MoneyBot.Services;
using Serilog;
using Serilog.Events;

namespace MoneyBot
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseConfigurationWithEnvironment()
                .UseSerilog((context, configuration) =>
                {
                    configuration
                        .MinimumLevel.Verbose()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
                })
                // .UseSimpleBotFramework((builder, context) =>
                // {
                //     builder.Services.AddScoped<TelegramContext>();
                //     builder.UseIdentity<User>();
                //     builder.Services.AddScoped<IUserRepository<User>, AccountRepository>();
                // })
                .UseSimpleBotFramework((app, context) =>
                {

                    app.Services.AddScoped<PeopleRepository>();
                    app.Services.AddScoped<PeopleService>();
                    app.Services.AddScoped<TelegramContext>();
                    app.Services.AddScoped<IUserRepository<User>, AccountRepository>();
                    
                    app.UseMiddleware<LoggingMiddleware>();
                    app.UseIdentity<User>();
                })
                .Build()
                .Run();
        }
    }
}