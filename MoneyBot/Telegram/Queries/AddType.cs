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
        public override OutMessage Execute(CallbackQuery message, Account account)
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

        public static OutMessage TypeCategory(Account account, Message message = null) => SelectType(account, true, message);
        public static OutMessage TypePerson(Account account, Message message = null) => SelectType(account, false, message);

        private static OutMessage SelectType(Account account, bool category, Message message = null)
        {
            if (category)
                account.CurrentExpense = new Expense();
            else
                account.CurrentTransaction = new Transaction();

            var text = category? "ExpenseType": "TransactionType";
            var replyMarkup = Keyboards.CategoryTypes(text);
            if (message == null)
            {
                return new OutMessage(account, $"Choose one:", replyMarkup : replyMarkup);
            }
            else
                return new OutMessage(account, $"Choose one:", replyMarkup : replyMarkup)
                {
                    EditMessageId = message.MessageId
                };
        }
    }
}