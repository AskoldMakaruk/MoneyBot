using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Clients.ClientExtensions;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class StartCommand : IStaticCommand
    {
        private readonly Account _account;

        public StartCommand(Account account)
        {
            _account = account;
        }

        public bool SuitableFirst(Update update)
        {
            return update.Message?.Text.StartsWith("/start") ?? false;
        }

        public async Task Execute(IClient client)
        {
            await client.SendTextMessage("Welcome to MoneyBot.", replyMarkup: Keyboards.MainKeyboard(_account));
        }
    }
}