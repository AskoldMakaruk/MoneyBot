using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class SelectTransactionTypeQuery : Query
    {
        public SelectTransactionTypeQuery(CallbackQuery message, Account account) : base(message, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("TransactionType");
        }
        public override OutMessage Execute()
        {
            var keyboard = Keyboards.People(Account.People, "AddTransaction");
            Account.CurrentTransaction.Type = Message.Data.ToLower().Contains("in") ? MoneyDirection.In : MoneyDirection.Out;
            return new OutMessage(Account, Message.Message.MessageId, $"Select person that {(Account.CurrentTransaction.Type == MoneyDirection.In?"gives money to you": "ownes you money")}:", replyMarkup : keyboard);
        }
    }
}