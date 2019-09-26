using MoneyBot.DB.Model;
using MoneyBot.DB.Secondary;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class AddTypeQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("AddType");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            if (message.Data.EndsWith("Category"))
                return SelectRecordType(account, RecordType.Expense, message.Message);
            else
                return SelectRecordType(account, RecordType.Transaction, message.Message);
        }

        public static Response SelectRecordType(Account account, RecordType category, Message message = null)
        {
            account.CurrentRecord = new AddRecord() { RecordType = category };
            var res = new Response(account, $"Choose one:", Keyboards.CategoryTypes("RecordType"));

            if (message != null)
                res.EditMessageId = message.MessageId;
            return res;
        }
    }
}