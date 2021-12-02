using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Services.Extensioins;
using Telegram.Bot.Types;
using User = MoneyBot.DB.Model.User;

namespace MoneyBot.Telegram.Commands
{
    public class StartCommand : IStaticCommand
    {
        private readonly User _user;

        public StartCommand(User user)
        {
            _user = user;
        }

        public bool SuitableFirst(Update update)
        {
            return update.Message?.Text.StartsWith("/start") ?? false;
        }

        public async Task Execute(IClient client)
        {
            await client.SendTextMessage("Welcome to MoneyBot.", replyMarkup: Keyboards.MainKeyboard(_user));
        }
    }
}