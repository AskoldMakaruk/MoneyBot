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
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.Free) res++;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            var Controller = account.Controller;
            account.Status = AccountStatus.Free;
            if (message.Text == "Manage Menu")
            {
                account.Status = AccountStatus.Manage;
                return new Response(account, "Do something", replyMarkup : Keyboards.Manage(account));
            }
            if (message.Text == "Add")
            {
                var keys = Keyboards.AddType(account);
                if (account.PeopleInited() && account.CategoriesInited())
                {
                    return new Response(account, $"This is about", replyMarkup : keys);
                }
                else if (account.PeopleInited())
                {
                    return AddTypeQuery.TypePerson(account);
                }
                else if (account.CategoriesInited())
                {
                    return AddTypeQuery.TypeCategory(account);
                }
                else
                    return new Response(account, $"Add category or person first");
            }
            if (message.Text == "Show")
            {
                if (account.PeopleInited() && account.CategoriesInited())
                {
                    account.Status = AccountStatus.ChooseShow;
                    return new Response(account, $"What you desire to see?", replyMarkup : Keyboards.MainShow);
                }
                else if (account.PeopleInited())
                {
                    return ShowCategoriesCommand.ToPeople(account);
                }
                else if (account.CategoriesInited())
                {
                    return ShowCategoriesCommand.ToCategory(account);
                }
                else
                    return new Response(account, $"Add category or person first");
            }
            if (message.Text == "Stats")
            {
                //todo hide empty categories
                var stats = Controller.GetStats(account.Id);
                var mes =
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
                return new Response(account, mes);
            }
            if (message.Text.StartsWith("/start"))
            {
                return new Response(account, "Welcome to MoneyBot.", replyMarkup : Keyboards.MainKeyboard(account));
            }
            if (message.Text == "deletedb" && account.ChatId == 249258727)
            {
                Controller.DeleteDb();
                return new Response(account, "Beep boop.");
            }
            if (message.Text.ToLower() == "deleteme" && account.ChatId == 249258727)
            {
                Controller.RemoveAccount(account);
                return new Response(account, "You were deleted.");
            }
            var regex = new Regex("(.{0,} - .{0,} - [0123456789.]{0,})");
            var added = message.Text
                .Split('\n')
                .Where(m => regex.Match(m).Success)
                .Select(m => new
                {
                    Category = account.Categories
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
                account.Controller.SaveChanges();
                return new Response(account, $"{builder.ToString()}", replyMarkup : Keyboards.MainKeyboard(account));
            }
            return new Response(account, $"Hi!", replyMarkup : Keyboards.MainKeyboard(account));
        }
    }
}