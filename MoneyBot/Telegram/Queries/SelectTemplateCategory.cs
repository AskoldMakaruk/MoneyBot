using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Queries
{
    public class SelectTemplateCategoryQuery : Query
    {

        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("AddTemplate");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            if (!message.Data.TryParseId(out var categoryId))
            {
                return new Response(message.Id, "Internal error");

            }
            account.CurrentTemplate.Category = account.Categories.First(c => c.Id == categoryId);
            var templates = account.CurrentTemplate.Category.Templates;
            account.Status = AccountStatus.EnterTemplate;
            return new Response(
                account,
                message.Message.MessageId,
                $"Adding template to {account.CurrentTemplate.Category.ToString()}\n" +
                $@"Enter new template in format:
[Name] - [sum]

Example:
Pork - 229.33");

        }
    }
}