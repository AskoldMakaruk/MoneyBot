using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Extensions;
using BotFramework.Services.Extensioins;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
using User = MoneyBot.DB.Model.User;

namespace MoneyBot.Telegram.Commands
{
    public class ManageMenuCommand : IStaticCommand
    {
        private readonly User _user;

        public ManageMenuCommand(User user)
        {
            _user = user;
        }

        public bool SuitableFirst(Update update) => update?.Message?.Text == "Manage Menu";

        public async Task Execute(IClient client)
        {
            await client.SendTextMessage("Do something", replyMarkup: Keyboards.Manage(_user));
        }
    }

    public class AdminCommands : IStaticCommand
    {
        private readonly User _user;

        public AdminCommands(User user)
        {
            _user = user;
        }

        public bool SuitableFirst(Update update)
        {
            return false;
            return update.GetId() == 249258727;
        }

        public async Task Execute(IClient client)
        {
            var message = await client.GetTextMessage();
            if (_user.Id == 249258727)
            {
                if (message.Text.ToLower() == "deletedb")
                {
                    // controller.DeleteDb();
                    // await client.SendTextMessage(_user, "Beep boop.");
                }

                if (message.Text.ToLower() == "deleteme")
                {
                    // controller.RemoveAccount(_user);
                    // await client.SendTextMessage(_user, "You were deleted.");
                }

                if (message.Text.ToLower() == "refreshme")
                {
                    // AccountRepository.Accounts.Remove(_user.ChatId);
                    // await client.SendTextMessage(_user, "Feeling refreshed?");
                }
            }
        }
    }

    public class MainCommand : IStaticCommand
    {
        private readonly User _user;

        public MainCommand(User user)
        {
            _user = user;
        }

        // public bool SuitableLast(Update update) => true;

        public async Task Execute(IClient client)
        {
            var message = await client.GetTextMessage();

            var regex = new Regex("(.{0,} - .{0,} - [0123456789.]{0,})");
            var added = message.Text
                .Split('\n')
                .Where(m => regex.Match(m).Success)
                .Select(m => new
                {
                    Category = _user.Categories.FirstOrDefault(c => m.StartsWith(c.Emoji)),
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

                // _user.Controller.SaveChanges();
                // await client.SendTextMessage(user, $"{builder.ToString()}", replyMarkup: Keyboards.MainKeyboard(user));
            }

            // await client.SendTextMessage(user, $"Hi!", replyMarkup: Keyboards.MainKeyboard(user));
        }
    }
}