using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class SelectTransactionPersonQuery : Query
    {
        public SelectTransactionPersonQuery(CallbackQuery message, Bot client, Account account) : base(message, client, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("AddTransaction");
        }
        public override async void Execute()
        {
            if (Account.CurrentTransaction == null) return;
            if (!Message.Data.TryParseId(out var personId))
            {
                await Client.AnswerCallbackQueryAsync(Message.Id, "Internal error");
                return;
            }
            Account.CurrentTransaction.Person = Account.People.First(c => c.Id == personId);
            await Client.EditMessageTextAsync(
                Account.ChatId,
                Message.Message.MessageId,
                $"Adding transaction between you and{Account.CurrentTransaction.Person.Name}\n" +
                $@"Enter details in format:
[Description] - [sum]

Example:
For vodka - 50");
            Account.Status = AccountStatus.EnterTransactionSum;
        }
    }
}