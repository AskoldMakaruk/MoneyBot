using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoneyBot.Controllers;
using MoneyBot.DB.Model;
using MoneyBot.DB.Secondary;
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
        public bool Testing = false;
        public delegate void MessageSentHandler(OutMessage message);
        public event MessageSentHandler OnMessageSent;

        public Bot(string token) : base(token)
        {
            OnMessage += OnMessageRecieved;
            OnCallbackQuery += OnQueryReceived;
            //StartReceiving();
        }

        public void OnMessageRecieved(object sender, MessageEventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + e.Message.From.Username + ": " + e.Message.Text);
            try
            {
                HandleMessage(e.Message);
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

        public void HandleMessage(Message message)
        {
            var chatId = message.Chat.Id;
            Account account;

            if (TelegramController.Accounts.ContainsKey(chatId))
            {
                account = TelegramController.Accounts[chatId];
            }
            else
            {
                var contoller = new TelegramController();
                contoller.Start();
                account = contoller.FromMessage(message);
                account.Controller = contoller;
            }

            var command = GetCommand(message, account);
            var canceled = command.Canceled();

            Console.WriteLine($"Command: {command.ToString()}, status: {account.Status.ToString()}, canceled: {canceled}");

            SendTextMessageAsync(canceled?command.Relieve() : command.Execute());
        }

        protected Command GetCommand(Message message, Account account)
        {
            var baseType = typeof(Command);
            var assembly = baseType.Assembly;
            var command = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract).Select(c => Activator.CreateInstance(c, message, this, account) as Command).OrderByDescending(c => c.Suitability()).First();
            command.Controller = account.Controller;
            return command;
        }

        public async Task<Message> SendTextMessageAsync(OutMessage m)
        {
            if (Testing)
            {
                OnMessageSent?.Invoke(m);
                return new Message();
            }
            else if (m.EditMessageId == 0)
            {
                var message = await base.SendTextMessageAsync(m.Account, m.Text, replyToMessageId : m.ReplyToMessageId, replyMarkup : m.ReplyMarkup);
                m.Account.LastMessage = message;
                return message;
            }
            else
            {
                var message = await base.EditMessageTextAsync(m.Account, m.EditMessageId, m.Text, replyMarkup : m.ReplyMarkup as InlineKeyboardMarkup);
                m.Account.LastMessage = message;
                return message;
            }
        }

        public async Task<Message> SendTextMessageAsync(Account account, string text,
            ParseMode parseMode = ParseMode.Default, bool disableWebPagePreview = false, bool disableNotification = false,
            int replyToMessageId = 0, IReplyMarkup replyMarkup = null, CancellationToken cancellationToken = default)
        {
            if (Testing)
            {
                OnMessageSent?.Invoke(new OutMessage(account, text, replyMarkup, replyToMessageId));
                return new Message();
            }
            else
            {
                var message = await base.SendTextMessageAsync(account, text, parseMode, disableWebPagePreview, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
                account.LastMessage = message;
                return message;
            }
        }
    }
}