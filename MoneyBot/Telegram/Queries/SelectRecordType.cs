using System.Linq;
using MoneyBot.DB.Model;
using MoneyBot.DB.Secondary;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MoneyBot.Telegram.Queries
{
    public class SelectRecordTypeQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("RecordType");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            var record = account.CurrentRecord;
            if (record == null) return new Response(account, "InternalError");

            record.Direction = message.Data.EndsWith("In") ? MoneyDirection.In : MoneyDirection.Out;
            InlineKeyboardMarkup keyboard;

            string mes;
            if (record.RecordType == RecordType.Expense)
            {
                var categories = account.Categories.Where(c => c.Type == record.Direction);
                keyboard = Keyboards.Categories(categories, "AddRecord");
                mes = "Select expense category:";
            }
            else if (account.CurrentRecord?.RecordType == RecordType.Transaction)
            {
                keyboard = Keyboards.People(account.People, "AddRecord");
                mes = $"Select person that {(record.Direction == MoneyDirection.In?"gives money to you": "ownes you money")}:";
            }
            else
            {
                //todo default error message
                return new Response(account, "InternalError");
            }
            return new Response(account, message.Message.MessageId, mes, replyMarkup : keyboard);
        }
    }
}