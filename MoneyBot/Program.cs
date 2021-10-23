using BotFramework;
using BotFramework.Extensions.Hosting;
using BotFramework.HostServices;
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
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
                })
                // .UseSimpleBotFramework((builder, context) =>
                // {
                //     builder.Services.AddScoped<TelegramContext>();
                //     builder.UseIdentity<Account>();
                //     builder.Services.AddScoped<IUserRepository<Account>, AccountRepository>();
                // })
                .UseBotFramework((app, context) =>
                {

                    app.Services.AddScoped<PeopleRepository>();
                    app.Services.AddScoped<PeopleService>();
                    app.Services.AddScoped<TelegramContext>();
                    app.Services.AddScoped<IUserRepository<Account>, AccountRepository>();

                    app.Services.AddUpdateConsumer();
                    app.Services.AddTelegramClient(context.Configuration["BotToken"]);
                    
                    app.UseMiddleware<LoggingMiddleware>();
                    app.UseIdentity<Account>();

                    app.UseStaticCommands();
                    app.UseMiddleware<SuitableMiddleware>();
                })
                .Build()
                .Run();
        }
    }
}