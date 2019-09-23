using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class AddTypeQuery : Query
    {
        public AddTypeQuery(CallbackQuery message, Account account) : base(message, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("AddType");
        }
        public override OutMessage Execute()
        {
            if (Message.Data.EndsWith("Category"))
            {
                return TypeCategory(Account, Message.Message);

            }
            else
            {
                return TypePerson(Account, Message.Message);

            }
        }

        public static OutMessage TypeCategory(Account Account, Message Message = null) => SelectType(Account, true, Message);
        public static OutMessage TypePerson(Account Account, Message Message = null) => SelectType(Account, false, Message);

        private static OutMessage SelectType(Account Account, bool category, Message Message = null)
        {
            if (category)
                Account.CurrentExpense = new Expense();
            else
                Account.CurrentTransaction = new Transaction();

            var text = category? "ExpenseType": "TransactionType";
            var replyMarkup = Keyboards.CategoryTypes(text);
            if (Message == null)
            {
                return new OutMessage(Account, $"Choose one:", replyMarkup : replyMarkup);
            }
            else
                return new OutMessage(Account, $"Choose one:", replyMarkup : replyMarkup)
                {
                    EditMessageId = Message.MessageId
                };
        }
    }
}