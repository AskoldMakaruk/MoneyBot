using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Queries
{
    public class SelectExspenseCategoryQuery : Query
    {
        public SelectExspenseCategoryQuery(CallbackQuery message, Bot client, Account account) : base(message, client, account) { }
        public override bool IsSuitable()
        {
            return Message.Data.StartsWith("AddExspense");
        }
        public override async void Execute()
        {
            if (Account.CurrentExspense == null) return;
            if (!int.TryParse(Message.Data.Substring(Message.Data.IndexOf(" ") + 1), out var categoryId))
            {
                await Client.AnswerCallbackQueryAsync(Message.Id, "Internal error");
                return;
            }
            Account.CurrentExspense.Category = Account.Categories.First(c => c.Id == categoryId);
            await Client.SendTextMessageAsync(Account.ChatId, $"Adding exspense to {Account.CurrentExspense.Category.ToString()}\nEnter exspense sum:");
            Account.Status = AccountStatus.EnterExspenseSum;
        }
    }
}