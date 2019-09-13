using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class AddTypeQuery : Query
    {
        public AddTypeQuery(CallbackQuery message, Bot client, Account account) : base(message, client, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("AddType");
        }
        public override async void Execute()
        {
            if (Message.Data.EndsWith("Category"))
            {
                Account.CurrentExpense = new Expense();
                await Client.EditMessageTextAsync(Account.ChatId, Message.Message.MessageId, $"Choose one:", replyMarkup : Keyboards.CategoryTypes("ExpenseType"));
                return;
            }
            else
            {
                Account.CurrentTransaction = new Transaction();
                await Client.EditMessageTextAsync(Account.ChatId, Message.Message.MessageId, $"Choose one:", replyMarkup : Keyboards.CategoryTypes("TransactionType"));
                return;
            }
        }
    }
}