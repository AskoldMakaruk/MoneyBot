using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Services.Extensioins;
using Telegram.Bot.Types;
using User = MoneyBot.DB.Model.User;

namespace MoneyBot.Telegram.Commands
{
    public class ShowCommand : IStaticCommand
    {
        private readonly User _user;

        public ShowCommand(User user)
        {
            _user = user;
        }

        public bool SuitableFirst(Update update) => update.Message?.Text == "Show";

        public async Task Execute(IClient client)
        {
            if (_user.PeopleInitedAndNotEmpty() && _user.PeopleInitedAndNotEmpty())
            {
                // _user.Status = AccountStatus.ChooseShow;
                await client.SendTextMessage($"What you desire to see?", replyMarkup: Keyboards.MainShow);
            }
            else if (_user.PeopleInitedAndNotEmpty())
            {
                await client.ToPeople(_user);
            }
            else if (_user.CategoriesInitedAndNotEmpty())
            {
                await client.ToCategory(_user);
            }
            else
                await client.SendTextMessage($"Add category or person first");
        }
    }
}