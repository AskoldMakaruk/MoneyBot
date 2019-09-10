using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class SelectExpenseCategoryQuery : Query
    {
        public SelectExpenseCategoryQuery(CallbackQuery message, Bot client, Account account) : base(message, client, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("AddExpense");
        }
        public override async void Execute()
        {
            if (Account.CurrentExpense == null) return;
            if (!Message.Data.TryParseId(out var categoryId))
            {
                await Client.AnswerCallbackQueryAsync(Message.Id, "Internal error");
                return;
            }
            Account.CurrentExpense.Category = Account.Categories.First(c => c.Id == categoryId);
            var templates = Account.CurrentExpense.Category.Templates;
            await Client.EditMessageTextAsync(
                Account.ChatId,
                Message.Message.MessageId,
                $"Adding expense to {Account.CurrentExpense.Category.ToString()}\n" +
                $@"Enter new expense in format:
[Description] - [sum]

Example:
Pork - 229.33{(templates != null?"\n\nOr use template:":"")}", replyMarkup : templates != null? Keyboards.Templates(templates, "Template") : null);
            Account.Status = AccountStatus.EnterExpenseSum;
        }
    }
}