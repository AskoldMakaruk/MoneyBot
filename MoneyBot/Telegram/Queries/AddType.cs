using System.Threading.Tasks;
using BotFramework.Abstractions;
using MoneyBot.DB.Model;
using MoneyBot.DB.Secondary;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class AddTypeQuery : IStaticCommand
    {
        private readonly Account _account;

        public AddTypeQuery(Account account)
        {
            _account = account;
        }

        public bool SuitableFirst(Update update) => update.CallbackQuery?.Data?.StartsWith("AddType") ?? false;

        public async Task Execute(IClient client)
        {
            var message = (await client.GetUpdate()).CallbackQuery;

            if (message.Data.EndsWith("Category"))
                await client.SelectRecordType(_account, RecordType.Expense, message.Message);
            else
                await client.SelectRecordType(_account, RecordType.Transaction, message.Message);
        }
    }
}