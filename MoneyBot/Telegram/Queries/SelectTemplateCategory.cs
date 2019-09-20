using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class SelectTemplateCategoryQuery : Query
    {
        public SelectTemplateCategoryQuery(CallbackQuery message, Bot client, Account account) : base(message, client, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("AddTemplate");
        }
        public override async void Execute()
        {
            if (!Message.Data.TryParseId(out var categoryId))
            {
                await Client.AnswerCallbackQueryAsync(Message.Id, "Internal error");
                return;
            }
            Account.CurrentTemplate.Category = Account.Categories.First(c => c.Id == categoryId);
            var templates = Account.CurrentTemplate.Category.Templates;
            await Client.EditMessageTextAsync(
                Account.ChatId,
                Message.Message.MessageId,
                $"Adding template to {Account.CurrentTemplate.Category.ToString()}\n" +
                $@"Enter new template in format:
[Name] - [sum]

Example:
Pork - 229.33");
            Account.Status = AccountStatus.EnterTemplate;
        }
    }
}