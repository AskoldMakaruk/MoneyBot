using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Clients.ClientExtensions;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class ShowCommand : IStaticCommand
    {
        private readonly Account _account;

        public ShowCommand(Account account)
        {
            _account = account;
        }

        public bool SuitableFirst(Update update) => update.Message?.Text == "Show";

        public async Task Execute(IClient client)
        {
            if (_account.PeopleInitedAndNotEmpty() && _account.PeopleInitedAndNotEmpty())
            {
                _account.Status = AccountStatus.ChooseShow;
                await client.SendTextMessage($"What you desire to see?", replyMarkup: Keyboards.MainShow);
            }
            else if (_account.PeopleInitedAndNotEmpty())
            {
                await client.ToPeople(_account);
            }
            else if (_account.CategoriesInitedAndNotEmpty())
            {
                await client.ToCategory(_account);
            }
            else
                await client.SendTextMessage($"Add category or person first");
        }
    }
}