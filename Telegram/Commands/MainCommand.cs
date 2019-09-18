﻿using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            if (Message.Text == "Add")
            {
                var keys = Keyboards.AddType(Account);
                if (keys.InlineKeyboard.Count() != 0)
                    await Client.SendTextMessageAsync(Account.ChatId, $"This is about", replyMarkup : keys);
                else
                    await Client.SendTextMessageAsync(Account.ChatId, $"Add category or person first");
                return;
            }
            if (Message.Text == "Show")
            {
                Account.Status = AccountStatus.ChooseShow;
                await Client.SendTextMessageAsync(Account.ChatId, $"What you desire to see?", replyMarkup : Keyboards.MainShow);
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
            if (Message.Text == "deletedb" && Account.ChatId == 249258727)
            {
                Controller.DeleteDb();
            }

            var regex = new Regex("(.{0,} - .{0,} - [0123456789.]{0,})");
            var added = Message.Text
                .Split("\n")
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
                await Client.SendTextMessageAsync(Account.ChatId, $"{builder.ToString()}", replyMarkup : Keyboards.Main);
                Account.Controller.SaveChanges();
                return;
            }
            await Client.SendTextMessageAsync(Account.ChatId, $"Hi!", replyMarkup : Keyboards.Main);
        }
    }
}