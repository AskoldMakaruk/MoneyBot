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