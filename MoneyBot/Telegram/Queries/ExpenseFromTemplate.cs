using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class ExepenseFromTemplateQuery : Query
    {
        public ExepenseFromTemplateQuery(CallbackQuery message, Account account) : base(message, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("Template");
        }
        public override OutMessage Execute()
        {
            if (Message.Data.TryParseId(out var template))
            {
                Controller.AddExpense(template);
                Account.Status = AccountStatus.Free;
                return new OutMessage(Account, "Success!") { EditMessageId = Message.Message.MessageId };
            }
            else return new OutMessage(Message.Id, "Server error: template not found");

        }
    }
}