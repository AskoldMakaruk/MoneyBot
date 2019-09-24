using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class SelectExpenseTypeQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("ExpenseType");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            var categories = account.Categories.Where(c => c.Type == (message.Data.EndsWith("In") ? MoneyDirection.In : MoneyDirection.Out));
            var keyboard = Keyboards.Categories(categories, "AddExpense");
            return new Response(account, message.Message.MessageId, $"Select expense category:", replyMarkup : keyboard);
        }
    }
}