// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using BotFramework.Abstractions;
// using BotFramework.Clients.ClientExtensions;
// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
//
// namespace MoneyBot.Telegram.Queries
// {
//     public class ShowCategoryQuery : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             return message.Data.StartsWith("ShowCategory");
//         }
//         public async Task Execute(IClient client)
//         {
//             account.Status = AccountStatus.Free;
//
//             message.Data.TryParseId(out var id);
//
//             var category = account.Categories?.First(ct => ct.Id == id);
//             if (category == null || category?.Expenses == null)
//             {
//                 await client.SendTextMessage(message.Id, "Everything is null");
//             }
//
//             var categoryDays = category.Expenses.GroupBy(e => e.Date.Date)
//                 .Select(r => $"{r.Key.ToString("dd MMMM")}:\n{string.Join("\n", r.Select(k => k.Description.IsNullOrEmpty()?k.Sum.ToString(): $"{k.Description}: {k.Sum}"))}");
//
//             string mes = $"{category.ToString()}\n{string.Join("\n"+new string('-', 10)+"\n", categoryDays)}".Trim();
//
//             if (message.Message.Text != mes)
//             {
//                 await client.SendTextMessage(account, message.Message.MessageId, mes, replyMarkup : Keyboards.ShowActiveCategories(account.Categories));
//             }
//             else await client.SendTextMessage(message.Id, null);
//         }
//     }
// }