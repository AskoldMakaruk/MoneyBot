using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class SelectTransactionTypeQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("TransactionType");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            var keyboard = Keyboards.People(account.People, "AddTransaction");
            account.CurrentTransaction.Type = message.Data.ToLower().Contains("in") ? MoneyDirection.In : MoneyDirection.Out;
            return new Response(account, message.Message.MessageId, $"Select person that {(account.CurrentTransaction.Type == MoneyDirection.In?"gives money to you": "ownes you money")}:", replyMarkup : keyboard);
        }
    }
}