using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Clients.ClientExtensions;
using MoneyBot.Controllers;
using MoneyBot.DB.Model;
using MoneyBot.Telegram.Queries;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class ManageMenuCommand : IStaticCommand
    {
        private readonly AccountRepository _accountRepository;

        public ManageMenuCommand(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool SuitableFirst(Update update) => update?.Message?.Text == "Manage Menu";

        public async Task Execute(IClient client)
        {
            var update = await client.GetTextMessage();
            var account = _accountRepository.FromMessage(update);

            await client.SendTextMessage("Do something", replyMarkup: Keyboards.Manage(account));
            
            //todo handle keyboard responses
        }
    }

    public class MainCommand : IStaticCommand
    {
    }

    public class MainCommand : IStaticCommand
    {
        private readonly AccountRepository _accountRepository;

        public MainCommand(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool SuitableLast(Update update) => true;

        public async Task Execute(IClient client)
        {
            var update = await client.GetTextMessage();
            var account = _accountRepository.FromMessage(update);

            var controller = account.Controller;

            if (account.ChatId == 249258727)
            {
                if (message.Text.ToLower() == "deletedb")
                {
                    controller.DeleteDb();
                    return new Response(account, "Beep boop.");
                }

                if (message.Text.ToLower() == "deleteme")
                {
                    controller.RemoveAccount(account);
                    return new Response(account, "You were deleted.");
                }

                if (message.Text.ToLower() == "refreshme")
                {
                    AccountRepository.Accounts.Remove(account.ChatId);
                    return new Response(account, "Feeling refreshed?");
                }
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
                return new Response(account, $"{builder.ToString()}", replyMarkup: Keyboards.MainKeyboard(account));
            }

            return new Response(account, $"Hi!", replyMarkup: Keyboards.MainKeyboard(account));
        }
    }
}