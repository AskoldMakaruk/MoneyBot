using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class AddTypeQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("AddType");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            if (message.Data.EndsWith("Category"))
            {
                return TypeCategory(account, message.Message);
            }
            else
            {
                return TypePerson(account, message.Message);
            }
        }

        public static Response TypeCategory(Account account, Message message = null) => SelectType(account, true, message);
        public static Response TypePerson(Account account, Message message = null) => SelectType(account, false, message);

        private static Response SelectType(Account account, bool category, Message message = null)
        {
            if (category)
                account.CurrentExpense = new Expense();
            else
                account.CurrentTransaction = new Transaction();

            var text = category? "ExpenseType": "TransactionType";
            var replyMarkup = Keyboards.CategoryTypes(text);
            if (message == null)
            {
                return new Response(account, $"Choose one:", replyMarkup : replyMarkup);
            }
            else
                return new Response(account, $"Choose one:", replyMarkup : replyMarkup)
                {
                    EditMessageId = message.MessageId
                };
        }
    }
}