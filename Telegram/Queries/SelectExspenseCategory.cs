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
            await Client.SendTextMessageAsync(Account.ChatId, $"Adding exspense to {Account.CurrentExspense.Category.ToString()}\nEnter exspense sum:");
            Account.Status = AccountStatus.EnterExspenseSum;
        }
    }
}