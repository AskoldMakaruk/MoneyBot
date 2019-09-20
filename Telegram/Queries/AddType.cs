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
        public override void Execute()
        {
            if (Message.Data.EndsWith("Category"))
            {
                TypeCategory(Account, Client, Message.Message);
                return;
            }
            else
            {
                TypePerson(Account, Client, Message.Message);
                return;
            }
        }

        public static void TypeCategory(Account Account, Bot Client, Message Message = null) => SelectType(Account, Client, true, Message);
        public static void TypePerson(Account Account, Bot Client, Message Message = null) => SelectType(Account, Client, false, Message);

        private static async void SelectType(Account Account, Bot Client, bool category, Message Message = null)
        {
            if (category)
                Account.CurrentExpense = new Expense();
            else
                Account.CurrentTransaction = new Transaction();

            var text = category? "ExpenseType": "TransactionType";
            var replyMarkup = Keyboards.CategoryTypes(text);
            if (Message == null)
            {
                await Client.SendTextMessageAsync(Account.ChatId, $"Choose one:", replyMarkup : replyMarkup);
            }
            else
                await Client.EditMessageTextAsync(Account.ChatId, Message.MessageId, $"Choose one:", replyMarkup : replyMarkup);
        }
    }
}