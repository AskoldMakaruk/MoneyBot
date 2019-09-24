using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class ExepenseFromTemplateQuery : Query
    {
        public override bool IsSuitable(CallbackQuery message, Account account)
        {
            return message.Data.StartsWith("Template");
        }
        public override Response Execute(CallbackQuery message, Account account)
        {
            if (message.Data.TryParseId(out var template))
            {
                Controller.AddExpense(template);
                account.Status = AccountStatus.Free;
                return new Response(account, "Success!") { EditMessageId = message.Message.MessageId };
            }
            else return new Response(message.Id, "Server error: template not found");

        }
    }
}