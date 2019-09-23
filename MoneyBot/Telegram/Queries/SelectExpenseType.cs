using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class SelectExpenseTypeQuery : Query
    {
        public SelectExpenseTypeQuery(CallbackQuery message, Account account) : base(message, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("ExpenseType");
        }
        public override OutMessage Execute()
        {
            var categories = Account.Categories.Where(c => c.Type == (Message.Data.EndsWith("In") ? MoneyDirection.In : MoneyDirection.Out));
            var keyboard = Keyboards.Categories(categories, "AddExpense");
            return new OutMessage(Account, Message.Message.MessageId, $"Select expense category:", replyMarkup : keyboard);
        }
    }
}