using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class AddExpenseCommand : Command
    {
        public AddExpenseCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Message.Text == "Add expense") res += 2;
            if (Account.Status == AccountStatus.Free) res++;
            return res;
        }
        public override async void Execute()
        {
            var categories = Controller.GetCategories(Account.Id);
            var keyboard = Keyboards.Categories(categories, "AddExpense");
            Account.CurrentExpense = new Expense();
            await Client.SendTextMessageAsync(Account.ChatId, $"Select expense category:", replyMarkup : keyboard);
        }
    }
}