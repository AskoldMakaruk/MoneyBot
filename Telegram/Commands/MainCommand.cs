using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class MainCommand : Command
    {
        public MainCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }

        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Free) res++;
            if (Message.Text != null) res++;
            return res;
        }
        public override async void Execute()
        {

            if (Message.Text == "Manage Menu")
            {
                await Client.SendTextMessageAsync(Account.ChatId, "Do something", replyMarkup : Keyboards.Manage);
                Account.Status = AccountStatus.Manage;
                return;
            }
            if (Message.Text == "Add expense")
            {
                Account.CurrentExpense = new Expense();
                await Client.SendTextMessageAsync(Account.ChatId, $"Choose one:", replyMarkup : Keyboards.CategoryTypes);
                return;
            }
            if (Message.Text == "Stats")
            {

            }
            if (Message.Text.StartsWith("/start"))
            {
                await Client.SendTextMessageAsync(Account.ChatId, "Welcome to MoneyBot.", replyMarkup : Keyboards.Main);
                return;
            }
            await Client.SendTextMessageAsync(Account.ChatId, $"Hi!", replyMarkup : Keyboards.Main);
        }
    }
}