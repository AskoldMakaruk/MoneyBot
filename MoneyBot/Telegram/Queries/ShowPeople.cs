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
//     public class ShowPeopleQuery : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             return message.Data.StartsWith("ShowPeople");
//         }
//         public async Task Execute(IClient client)
//         {
//             message.Data.TryParseId(out var id);
//
//             var person = account.People?.First(ct => ct.Id == id);
//
//             if (person?.Transactions == null)
//             {
//                 await client.SendTextMessage(message.Id, "Everything is null");
//             }
//             account.Status = AccountStatus.Free;
//             var categoryDays = person.Transactions.GroupBy(e => e.Date.Date).Select(r => $"{r.Key.ToString("dd MMMM")}\n{string.Join("\n", r.Select(k => $"{k.Description}: {(k.Type == MoneyDirection.In?"+":"-")+k.Sum}"))}");
//
//             string mes = $"{person.Name}\n{string.Join("\n"+new string('-', 10)+"\n", categoryDays)}".Trim();
//             if (message.Message.Text != mes)
//                 await client.SendTextMessage(account, mes, replyMarkup : Keyboards.People(account.People.ToArray(), "ShowPeople"))
//                 {
//                     EditMessageId = message.Message.MessageId
//                 };
//             else await client.SendTextMessage(message.Id, null);
//         }
//     }
// }