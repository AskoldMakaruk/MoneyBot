using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class SelectTransactionPersonQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("AddTransaction");
        }
        public override OutMessage Execute(CallbackQuery message, Account account)
        {
            if (account.CurrentTransaction == null) return new OutMessage(message.Id, "Something went wrong true again");
            if (!message.Data.TryParseId(out var personId))
            {
                return new OutMessage(message.Id, "Internal error");
            }
            account.CurrentTransaction.Person = account.People.First(c => c.Id == personId);
            account.Status = AccountStatus.EnterTransactionSum;
            return new OutMessage(
                account,
                message.Message.MessageId,
                $"Adding transaction between you and{account.CurrentTransaction.Person.Name}\n" +
                $@"Enter details in format:
[Description] - [sum]

Example:
For vodka - 50");

        }
    }
}