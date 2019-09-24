using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class SelectExpenseCategoryQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("AddExpense");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            if (account.CurrentExpense == null) return new Response(message.Id, "You have no expenses");
            if (!message.Data.TryParseId(out var categoryId))
            {
                return new Response(message.Id, "Internal error");
            }
            account.CurrentExpense.Category = account.Categories.First(c => c.Id == categoryId);
            var templates = account.CurrentExpense.Category.Templates;
            account.Status = AccountStatus.EnterExpenseSum;
            return new Response(
                account,
                $"Adding expense to {account.CurrentExpense.Category.ToString()}\n" +
                $@"Enter new expense in format:
[Description] - [sum]

Example:
Pork - 229.33{(templates != null && templates.Count>0?"\n\nOr use template:":"")}", replyMarkup : templates != null? Keyboards.Templates(templates, "Template") : null)
            {
                EditMessageId = message.Message.MessageId
            };

        }
    }
}