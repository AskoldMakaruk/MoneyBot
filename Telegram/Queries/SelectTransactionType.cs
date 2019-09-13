using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class SelectTransactionTypeQuery : Query
    {
        public SelectTransactionTypeQuery(CallbackQuery message, Bot client, Account account) : base(message, client, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("TransactionType");
        }
        public override async void Execute()
        {
            var keyboard = Keyboards.People(Account.People, "AddTransaction");
            await Client.EditMessageTextAsync(Account.ChatId, Message.Message.MessageId, $"Select person that {(Account.CurrentTransaction.Type == MoneyDirection.In?"gives money to you": "ownes you money")}:", replyMarkup : keyboard);
        }
    }
}