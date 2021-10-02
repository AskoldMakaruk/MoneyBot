using BotFramework.Abstractions;
using BotFramework.Clients;
using BotFramework.HostServices;
using BotFramework.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace MoneyBot
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseConfigurationWithEnvironment()
                .ConfigureApp((app, context) =>
                {
                    app.Services.AddSingleton<ITelegramBotClient>(_ =>
                        // new TelegramBotClient(context.Configuration["BotToken"]));
                        new TelegramBotClient("823973981:AAGYpq1Eyl_AAYGXLeW8s28uCH89S7fsHZA"));
                    app.Services.AddTransient<IUpdateConsumer, Client>();
                    app.UseHandlers();
                    app.UseStaticCommands();
                })
                .Build()
                .Run();
        }
    }
}