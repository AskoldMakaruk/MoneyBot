using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class SelectExpenseCategoryQuery : Query
    {
        public SelectExpenseCategoryQuery(CallbackQuery message, Account account) : base(message, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("AddExpense");
        }
        public override OutMessage Execute()
        {
            if (Account.CurrentExpense == null) return new OutMessage(Message.Id, "You have no expenses");
            if (!Message.Data.TryParseId(out var categoryId))
            {
                return new OutMessage(Message.Id, "Internal error");
            }
            Account.CurrentExpense.Category = Account.Categories.First(c => c.Id == categoryId);
            var templates = Account.CurrentExpense.Category.Templates;
            Account.Status = AccountStatus.EnterExpenseSum;
            return new OutMessage(
                Account,
                $"Adding expense to {Account.CurrentExpense.Category.ToString()}\n" +
                $@"Enter new expense in format:
[Description] - [sum]

Example:
Pork - 229.33{(templates != null && templates.Count>0?"\n\nOr use template:":"")}", replyMarkup : templates != null? Keyboards.Templates(templates, "Template") : null)
            {
                EditMessageId = Message.Message.MessageId
            };

        }
    }
}