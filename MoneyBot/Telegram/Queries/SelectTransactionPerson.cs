using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class SelectTransactionPersonQuery : Query
    {
        public SelectTransactionPersonQuery(CallbackQuery message, Account account) : base(message, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("AddTransaction");
        }
        public override OutMessage Execute()
        {
            if (Account.CurrentTransaction == null) return new OutMessage(Message.Id, "Something went wrong true again");
            if (!Message.Data.TryParseId(out var personId))
            {
                return new OutMessage(Message.Id, "Internal error");
            }
            Account.CurrentTransaction.Person = Account.People.First(c => c.Id == personId);
            Account.Status = AccountStatus.EnterTransactionSum;
            return new OutMessage(
                Account,
                Message.Message.MessageId,
                $"Adding transaction between you and{Account.CurrentTransaction.Person.Name}\n" +
                $@"Enter details in format:
[Description] - [sum]

Example:
For vodka - 50");

        }
    }
}