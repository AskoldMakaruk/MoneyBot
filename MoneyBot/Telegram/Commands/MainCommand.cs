using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MoneyBot.DB.Model;
using MoneyBot.Telegram.Queries;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class MainCommand : Command
    {
        public MainCommand(Message message, Account Account) : base(message, Account) { }

        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Free) res++;
            return res;
        }
        public override OutMessage Execute()
        {
            Account.Status = AccountStatus.Free;
            if (Message.Text == "Manage Menu")
            {
                Account.Status = AccountStatus.Manage;
                return new OutMessage(Account, "Do something", replyMarkup : Keyboards.Manage(Account));
            }
            if (Message.Text == "Add")
            {
                var keys = Keyboards.AddType(Account);
                if (Account.PeopleInited() && Account.CategoriesInited())
                {
                    return new OutMessage(Account, $"This is about", replyMarkup : keys);
                }
                else if (Account.PeopleInited())
                {
                    return AddTypeQuery.TypePerson(Account);
                }
                else if (Account.CategoriesInited())
                {
                    return AddTypeQuery.TypeCategory(Account);
                }
                else
                    return new OutMessage(Account, $"Add category or person first");
            }
            if (Message.Text == "Show")
            {
                if (Account.PeopleInited() && Account.CategoriesInited())
                {
                    Account.Status = AccountStatus.ChooseShow;
                    return new OutMessage(Account, $"What you desire to see?", replyMarkup : Keyboards.MainShow);
                }
                else if (Account.PeopleInited())
                {
                    return ShowCategoriesCommand.ToPeople(Account);
                }
                else if (Account.CategoriesInited())
                {
                    return ShowCategoriesCommand.ToCategory(Account);
                }
                else
                    return new OutMessage(Account, $"Add category or person first");
            }
            if (Message.Text == "Stats")
            {
                //todo hide empty categories
                var stats = Controller.GetStats(Account.Id);
                var message =
                    $@"Your stats
{(DateTime.Now - new TimeSpan(30,0,0,0)).ToString("dd MMMM")} - {DateTime.Now.ToString("dd MMMM")}
Balance: {stats.Balance}
Ownings: {stats.Owe}
Incomes: {stats.Incomes}
{string.Join("\n",stats.TopIncomeCategories.Select(c => $"{c.Emoji}{c.Name} {c.Expenses.Sum(r => r.Sum)}"))}
Expenses: {stats.Expenses}
{string.Join("\n",stats.TopExpenseCategories.Select(c => $"{c.Emoji}{c.Name} {c.Expenses.Sum(r => r.Sum)}"))}

Your top creditors:
{string.Join("\n",stats.TopCreditors.Select(d => $"{d.Name}: {d.CountSum()}"))}

Your top debtors:
{string.Join("\n",stats.TopDeptors.Select(d => $"{d.Name}: {d.CountSum()}"))}
";
                return new OutMessage(Account, message);
            }
            if (Message.Text.StartsWith("/start"))
            {
                return new OutMessage(Account, "Welcome to MoneyBot.", replyMarkup : Keyboards.MainKeyboard(Account));
            }
            if (Message.Text == "deletedb" && Account.ChatId == 249258727)
            {
                Controller.DeleteDb();
                return new OutMessage(Account, "Beep boop.");
            }
            if (Message.Text.ToLower() == "deleteme" && Account.ChatId == 249258727)
            {
                Controller.RemoveAccount(Account);
                return new OutMessage(Account, "You were deleted.");
            }
            var regex = new Regex("(.{0,} - .{0,} - [0123456789.]{0,})");
            var added = Message.Text
                .Split('\n')
                .Where(m => regex.Match(m).Success)
                .Select(m => new
                {
                    Category = Account.Categories
                        .FirstOrDefault(c => m.StartsWith(c.Emoji)),
                        Message = m
                })
                .Where(c => c.Category != null);

            if (added.Count() != 0)
            {
                var builder = new StringBuilder();
                foreach (var a in added)
                {
                    var values = a.Message.TrimDoubleSpaces().TrySplit('-', ' ');
                    var expense = new Expense
                    {
                        Category = a.Category,
                        Date = DateTime.Now,
                        Description = values[1],
                        Sum = values[2].ParseSum()
                    };
                    a.Category.Expenses.Add(expense);
                    builder.Append($"{expense.Category.Emoji}: {expense.Sum}\n");
                }
                Account.Controller.SaveChanges();
                return new OutMessage(Account, $"{builder.ToString()}", replyMarkup : Keyboards.MainKeyboard(Account));
            }
            return new OutMessage(Account, $"Hi!", replyMarkup : Keyboards.MainKeyboard(Account));
        }
    }
}