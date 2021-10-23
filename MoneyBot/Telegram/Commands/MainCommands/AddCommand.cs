using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Clients.ClientExtensions;
using MoneyBot.DB.Model;
using MoneyBot.Telegram.Queries;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class AddCommand : IStaticCommand
    {
        private readonly Account _account;

        public AddCommand(Account account)
        {
            _account = account;
        }

        public bool SuitableFirst(Update update) => update?.Message?.Text == "Add";

        public async Task Execute(IClient client)
        {
            var update = await client.GetTextMessage();

            //todo fast templates and change text on buttons
            var keys = Keyboards.AddType(_account);
            if (_account.PeopleInited() && _account.CategoriesInited())
            {
                await client.SendTextMessage($"This is about", replyMarkup: keys);
            }
            else if (_account.PeopleInited())
            {
                await client.SelectRecordType(_account, DB.Secondary.RecordType.Transaction);
            }
            else if (_account.CategoriesInited())
            {
                await client.SelectRecordType(_account, DB.Secondary.RecordType.Expense);
            }
            else
            {
                await client.SendTextMessage($"Add category or person first", replyMarkup: keys);
            }
        }
    }
}