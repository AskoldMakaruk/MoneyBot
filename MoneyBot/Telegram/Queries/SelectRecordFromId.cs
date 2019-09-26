using System.Linq;
using System.Text;
using MoneyBot.DB.Model;
using MoneyBot.DB.Secondary;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MoneyBot.Telegram.Queries
{
    public class SelectRecordFromIdQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("AddRecord");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            var record = account.CurrentRecord;
            if (record == null) return new Response(message.Id, "You have no expenses");

            if (!message.Data.TryParseId(out var id))
            {
                return new Response(message.Id, "Internal error");
            }
            record.FromId = id;
            account.Status = AccountStatus.EnterRecordSum;

            string enterDetails = "Enter details in format:\n[Description] - [sum]\nOr\n[sum]";

            var builder = new StringBuilder();
            InlineKeyboardMarkup replyMarkup = null;
            if (record.RecordType == RecordType.Expense)
            {
                var category = account.Categories.First(c => c.Id == id);
                var templates = category.Templates;
                builder.AppendLine($"Adding expense to {category.ToString()}");
                builder.AppendLine(enterDetails);
                builder.AppendLine($"Example:\nPork - 229.33\n39.51");

                if (templates != null && templates.Count > 0)
                {
                    builder.AppendLine("Or use template:");
                    replyMarkup = Keyboards.Templates(templates, "Template");
                }
            }
            else if (record.RecordType == RecordType.Transaction)
            {
                var person = account.People.First(p => p.Id == id);
                builder.AppendLine($"Adding transaction between you and{person.Name}");
                builder.AppendLine(enterDetails);
                builder.AppendLine("Example:\nFor vodka - 50\n49");
            }
            else return new Response(message.Id, "Internal error");

            return new Response(account, builder.ToString(), replyMarkup)
            {
                EditMessageId = message.Message.MessageId
            };
        }
    }
}