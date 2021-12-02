// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using MoneyBot.Controllers;
// using MoneyBot.DB.Model;
// using MoneyBot.Telegram.Commands;
// using MoneyBot.Telegram.Queries;
// using Telegram.Bot;
// using Telegram.Bot.Args;
// using Telegram.Bot.Types;
// using Telegram.Bot.Types.Enums;
// using Telegram.Bot.Types.ReplyMarkups;
//
// namespace MoneyBot.Telegram
// {
//     public class Bot : TelegramBotClient
//     {
//         protected Dictionary<Func<Message, User, int>, Command> Commands { get; set; }
//
//         public Bot(string token) : base(token)
//         {
//             var baseType = typeof(Command);
//             var assembly = baseType.Assembly;
//
//             Commands = assembly
//                 .GetTypes()
//                 .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract)
//                 .Select(c => Activator.CreateInstance(c) as Command)
//                 .Where(a => a != null)
//                 .ToDictionary(x => new Func<Message, User, int>(x.Suitability), x => x);
//
//             OnMessage += OnMessageRecieved;
//             OnCallbackQuery += OnQueryReceived;
//         }
//
//         public async void HandleQuery(CallbackQuery query)
//         {
//             try
//             {
//                 var contoller = new TelegramController();
//                 contoller.Start();
//
//                 var user = contoller.FromMessage(query.Message.Chat);
//                 if (user == null)
//                 {
//                     await AnswerCallbackQueryAsync(query.Id, "Your user doesn't exist.");
//                     return;
//                 }
//
//                 var baseType = typeof(Query);
//                 var assembly = baseType.Assembly;
//
//                 var command = assembly.GetTypes()
//                     .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract)
//                     .Select(c => Activator.CreateInstance(c) as Query)
//                     .First(c => c.IsSuitable(query, user));
//
//                 command.Controller = contoller;
//
//                 Console.WriteLine($"Command: {command.ToString()}");
//
//                 await SendTextMessageAsync(command.Execute(query, user));
//             }
//             catch (Exception e)
//             {
//                 System.Console.WriteLine(e);
//             }
//         }
//
//         public async void HandleMessage(Message message)
//         {
//             var chatId = message.Chat.Id;
//             User user;
//
//             if (AccountRepository.Accounts.ContainsKey(chatId))
//             {
//                 user = AccountRepository.Accounts[chatId];
//             }
//             else
//             {
//                 var contoller = new TelegramController();
//                 contoller.Start();
//                 user = contoller.FromMessage(message);
//                 user.Controller = contoller;
//             }
//
//             var command = GetCommand(message, user);
//             var canceled = command.Canceled(message, user);
//
//             Console.WriteLine($"Command: {command.ToString()}, status: {user.Status.ToString()}, canceled: {canceled}");
//
//             await SendTextMessageAsync(canceled ? command.Relieve(message, user) : IStaticCommand.Execute(message, user));
//         }
//
//         protected Command GetCommand(Message message, User user)
//         {
//             return Commands[Commands.Keys.OrderByDescending(s => s.Invoke(message, user)).First()];
//         }
//
//         public async Task<Message> SendTextMessageAsync(Response m)
//         {
//             if (m.EditMessageId == 0)
//             {
//                 var message = await base.SendTextMessageAsync(m.User, m.Text, replyToMessageId: m.ReplyToMessageId, replyMarkup: m.ReplyMarkup);
//                 m.User.LastMessage = message;
//                 return message;
//             }
//             else
//             {
//                 var message = await base.EditMessageTextAsync(m.User, m.EditMessageId, m.Text, replyMarkup: m.ReplyMarkup as InlineKeyboardMarkup);
//                 m.User.LastMessage = message;
//                 return message;
//             }
//         }
//
//         public async Task<Message> SendTextMessageAsync(User user, string text,
//             ParseMode parseMode = ParseMode.Default, bool disableWebPagePreview = false, bool disableNotification = false,
//             int replyToMessageId = 0, IReplyMarkup replyMarkup = null, CancellationToken cancellationToken = default)
//         {
//             var message = await base.SendTextMessageAsync(user, text, parseMode, disableWebPagePreview, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
//             user.LastMessage = message;
//             return message;
//         }
//
//         public void OnMessageRecieved(object sender, MessageEventArgs e)
//         {
//             Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + e.Message.From.Username + ": " + e.Message.Text);
//             try
//             {
//                 HandleMessage(e.Message);
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine(ex);
//             }
//         }
//
//         public void OnQueryReceived(object sender, CallbackQueryEventArgs e)
//         {
//             Console.WriteLine(DateTime.Now.ToShortTimeString() + " " + e.CallbackQuery.From.Username + ": " + e.CallbackQuery.Data);
//             try
//             {
//                 HandleQuery(e.CallbackQuery);
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine(ex);
//             }
//         }
//     }
// }