using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoneyBot.Controllers;
using MoneyBot.DB.Model;
using MoneyBot.Telegram.Commands;
using MoneyBot.Telegram.Queries;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MoneyBot.Telegram
{
    public class Bot : TelegramBotClient
    {

        public Bot(string token) : base(token)
        {
            OnMessage += OnMessageRecieved;
            OnCallbackQuery += OnQueryReceived;
            StartReceiving();
        }

        public void OnMessageRecieved(object sender, MessageEventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + e.Message.From.Username + ": " + e.Message.Text);
            try
            {
                var chatId = e.Message.Chat.Id;
                Account account;

                if (TelegramController.Accounts.ContainsKey(chatId))
                {
                    account = TelegramController.Accounts[chatId];
                }
                else
                {
                    var contoller = new TelegramController();
                    contoller.Start();
                    account = contoller.FromMessage(e.Message);
                    account.Controller = contoller;
                }

                var baseType = typeof(Command);
                var assembly = baseType.Assembly;
                var command = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract).Select(c => Activator.CreateInstance(c, e.Message, this, account) as Command).OrderByDescending(c => c.Suitability()).First();
                command.Controller = account.Controller;
                var canceled = command.Canceled();
                Console.WriteLine($"Command: {command.ToString()}, status: {account.Status.ToString()}, canceled: {canceled}");
                if (canceled)
                    command.Relieve();
                else
                    command.Execute();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        public void OnQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            try
            {
                Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + e.CallbackQuery.From.Username + ": " + e.CallbackQuery.Data);
                var contoller = new TelegramController();
                contoller.Start();

                var account = contoller.FromMessage(e.CallbackQuery.Message.Chat);
                if (account == null)
                {
                    AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Your account doesn't exist.");
                    return;
                }

                var baseType = typeof(Query);
                var assembly = baseType.Assembly;

                var command = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract).Select(c => Activator.CreateInstance(c, e.CallbackQuery, this, account) as Query).First(c => c.IsSuitable());

                command.Controller = contoller;
                command.Execute();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        public async Task<Message> SendTextMessageAsync(Account account, string text,
            ParseMode parseMode = ParseMode.Default, bool disableWebPagePreview = false, bool disableNotification = false,
            int replyToMessageId = 0, IReplyMarkup replyMarkup = null, CancellationToken cancellationToken = default)
        {
            var message = await base.SendTextMessageAsync(account, text, parseMode, disableWebPagePreview, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
            account.LastMessage = message;
            return message;
        }

    }
}