using System;
using System.Linq;
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
            return res;
        }
        public override async void Execute()
        {
            Account.Status = AccountStatus.Free;
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
                var stats = Controller.GetStats(Account.Id);
                var message =
                    $@"Your stats
{(DateTime.Now - new TimeSpan(30,0,0,0)).ToString("dd MMMM")} - {DateTime.Now.ToString("dd MMMM")}
Balance: {stats.Balance}
Incomes: {stats.Incomes}
    {string.Join("\t\t\n",stats.TopIncomeCategories.Select(c => $"{c.Emoji}{c.Name} {c.Expenses.Sum(r => r.Sum)}"))}
Expenses: {stats.Expenses}
    {string.Join("\t\t\n",stats.TopExpenseCategories.Select(c => $"{c.Emoji}{c.Name} {c.Expenses.Sum(r => r.Sum)}"))}";
                await Client.SendTextMessageAsync(Account.ChatId, message);
                return;
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