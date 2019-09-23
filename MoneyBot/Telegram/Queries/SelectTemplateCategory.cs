using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class SelectTemplateCategoryQuery : Query
    {
        public SelectTemplateCategoryQuery(CallbackQuery message, Account account) : base(message, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("AddTemplate");
        }
        public override OutMessage Execute()
        {
            if (!Message.Data.TryParseId(out var categoryId))
            {
                return new OutMessage(Message.Id, "Internal error");

            }
            Account.CurrentTemplate.Category = Account.Categories.First(c => c.Id == categoryId);
            var templates = Account.CurrentTemplate.Category.Templates;
            Account.Status = AccountStatus.EnterTemplate;
            return new OutMessage(
                Account,
                Message.Message.MessageId,
                $"Adding template to {Account.CurrentTemplate.Category.ToString()}\n" +
                $@"Enter new template in format:
[Name] - [sum]

Example:
Pork - 229.33");

        }
    }
}