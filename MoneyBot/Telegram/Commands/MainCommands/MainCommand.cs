using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Clients.ClientExtensions;
using BotFramework.Helpers;
using MoneyBot.DB.Model;
using MoneyBot.Services;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class ManageMenuCommand : IStaticCommand
    {
        private readonly Account _account;

        public ManageMenuCommand(Account account)
        {
            _account = account;
        }

        public bool SuitableFirst(Update update) => update?.Message?.Text == "Manage Menu";

        public async Task Execute(IClient client)
        {
            await client.SendTextMessage("Do something", replyMarkup: Keyboards.Manage(_account));
        }
    }

    public class AdminCommands : IStaticCommand
    {
        private readonly Account _account;

        public AdminCommands(Account account)
        {
            _account = account;
        }

        public bool SuitableFirst(Update update)
        {
            return false;
            return update.GetId() == 249258727;
        }

        public async Task Execute(IClient client)
        {
            var message = await client.GetTextMessage();
            if (_account.ChatId == 249258727)
            {
                if (message.Text.ToLower() == "deletedb")
                {
                    // controller.DeleteDb();
                    // await client.SendTextMessage(_account, "Beep boop.");
                }

                if (message.Text.ToLower() == "deleteme")
                {
                    // controller.RemoveAccount(_account);
                    // await client.SendTextMessage(_account, "You were deleted.");
                }

                if (message.Text.ToLower() == "refreshme")
                {
                    // AccountRepository.Accounts.Remove(_account.ChatId);
                    // await client.SendTextMessage(_account, "Feeling refreshed?");
                }
            }
        }
    }

    public class MainCommand : IStaticCommand
    {
        private readonly Account _account;

        public MainCommand(Account account)
        {
            _account = account;
        }

        public bool SuitableLast(Update update) => true;

        public async Task Execute(IClient client)
        {
            var message = await client.GetTextMessage();

            var regex = new Regex("(.{0,} - .{0,} - [0123456789.]{0,})");
            var added = message.Text
                .Split('\n')
                .Where(m => regex.Match(m).Success)
                .Select(m => new
                {
                    Category = _account.Categories
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

                // _account.Controller.SaveChanges();
                // await client.SendTextMessage(account, $"{builder.ToString()}", replyMarkup: Keyboards.MainKeyboard(account));
            }

            // await client.SendTextMessage(account, $"Hi!", replyMarkup: Keyboards.MainKeyboard(account));
        }
    }
}