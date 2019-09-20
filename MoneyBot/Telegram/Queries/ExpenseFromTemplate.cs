using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class ExepenseFromTemplateQuery : Query
    {
        public ExepenseFromTemplateQuery(CallbackQuery message, Bot client, Account account) : base(message, client, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("Template");
        }
        public override async void Execute()
        {
            if (Message.Data.TryParseId(out var template))
            {
                Controller.AddExpense(template);
                await Client.EditMessageTextAsync(Account.ChatId, Message.Message.MessageId, "Success!");
                Account.Status = AccountStatus.Free;
            }
            else await Client.AnswerCallbackQueryAsync(Message.Id, "Server error: template not found");

        }
    }
}