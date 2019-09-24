using System;
using System.Collections.Generic;
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
        protected Dictionary<Func<Message, Account, int>, Command> Commands { get; set; }
        public Bot(string token) : base(token)
        {
            var baseType = typeof(Command);
            var assembly = baseType.Assembly;

            Commands = assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract)
                .Select(c => Activator.CreateInstance(c) as Command)
                .ToDictionary(x => new Func<Message, Account, int>(x.Suitability), x => x);

            OnMessage += OnMessageRecieved;
            OnCallbackQuery += OnQueryReceived;
            //StartReceiving();
        }

        public async void HandleQuery(CallbackQuery query)
        {
            var contoller = new TelegramController();
            contoller.Start();

            var account = contoller.FromMessage(query.Message.Chat);
            if (account == null)
            {
                await AnswerCallbackQueryAsync(query.Id, "Your account doesn't exist.");
                return;
            }

            var baseType = typeof(Query);
            var assembly = baseType.Assembly;

            var command = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract).Select(c => Activator.CreateInstance(c) as Query).First(c => c.IsSuitable(query, account));

            command.Controller = contoller;
            await SendTextMessageAsync(command.Execute(query, account));
        }

        public async void HandleMessage(Message message)
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
            var canceled = command.Canceled(message, account);

            Console.WriteLine($"Command: {command.ToString()}, status: {account.Status.ToString()}, canceled: {canceled}");

            await SendTextMessageAsync(canceled?command.Relieve(message, account) : command.Execute(message, account));
        }

        protected Command GetCommand(Message message, Account account)
        {
            return Commands[Commands.Keys.OrderByDescending(s => s.Invoke(message, account)).First()];
        }

        public async Task<Message> SendTextMessageAsync(Response m)
        {
            if (m.EditMessageId == 0)
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
            var message = await base.SendTextMessageAsync(account, text, parseMode, disableWebPagePreview, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
            account.LastMessage = message;
            return message;

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
            Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + e.CallbackQuery.From.Username + ": " + e.CallbackQuery.Data);
            try
            {
                HandleQuery(e.CallbackQuery);
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

    }
}