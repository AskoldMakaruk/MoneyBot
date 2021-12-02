using System.Threading.Tasks;
using BotFramework.Abstractions;
using MoneyBot.DB.Secondary;
using Telegram.Bot.Types;
using User = MoneyBot.DB.Model.User;

namespace MoneyBot.Telegram.Queries
{
    public class AddTypeQuery : IStaticCommand
    {
        private readonly User _user;

        public AddTypeQuery(User user)
        {
            _user = user;
        }

        public bool SuitableFirst(Update update) => update.CallbackQuery?.Data?.StartsWith("AddType") ?? false;

        public async Task Execute(IClient client)
        {
            var message = (await client.GetUpdate()).CallbackQuery;

            if (message.Data.EndsWith("Category"))
                await client.SelectRecordType(_user, RecordType.Expense, message.Message);
            else
                await client.SelectRecordType(_user, RecordType.Transaction, message.Message);
        }
    }
}