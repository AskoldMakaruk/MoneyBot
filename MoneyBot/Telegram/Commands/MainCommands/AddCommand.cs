using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Services.Extensioins;
using Telegram.Bot.Types;
using User = MoneyBot.DB.Model.User;

namespace MoneyBot.Telegram.Commands
{
    public class AddCommand : IStaticCommand
    {
        private readonly User _user;

        public AddCommand(User user)
        {
            _user = user;
        }

        public bool SuitableFirst(Update update) => update?.Message?.Text == "Add";

        public async Task Execute(IClient client)
        {
            var update = await client.GetTextMessage();

            //todo fast templates and change text on buttons
            var keys = Keyboards.AddType(_user);
            if (_user.PeopleInited() && _user.CategoriesInited())
            {
                await client.SendTextMessage($"This is about", replyMarkup: keys);
            }
            else if (_user.PeopleInited())
            {
                await client.SelectRecordType(_user, DB.Secondary.RecordType.Transaction);
            }
            else if (_user.CategoriesInited())
            {
                await client.SelectRecordType(_user, DB.Secondary.RecordType.Expense);
            }
            else
            {
                await client.SendTextMessage($"Add category or person first", replyMarkup: keys);
            }
        }
    }
}