// using System.Linq;
// using System.Threading.Tasks;
// using BotFramework.Abstractions;
// using BotFramework.Clients.ClientExtensions;
// using MoneyBot.DB.Model;
// using MoneyBot.DB.Secondary;
// using Telegram.Bot.Types;
// using Telegram.Bot.Types.ReplyMarkups;
//
// namespace MoneyBot.Telegram.Queries
// {
//     public class SelectRecordTypeQuery : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             return message.Data.StartsWith("RecordType");
//         }
//         public async Task Execute(IClient client)
//         {
//             var record = user.CurrentRecord;
//             if (record == null) await client.SendTextMessage(user, "InternalError");
//
//             record.Direction = message.Data.EndsWith("In") ? MoneyDirection.In : MoneyDirection.Out;
//             InlineKeyboardMarkup keyboard;
//
//             string mes;
//             if (record.RecordType == RecordType.Expense)
//             {
//                 var categories = user.Categories.Where(c => c.Type == record.Direction);
//                 keyboard = Keyboards.Categories(categories, "AddRecord");
//                 mes = "Select expense category:";
//             }
//             else if (user.CurrentRecord?.RecordType == RecordType.Transaction)
//             {
//                 keyboard = Keyboards.People(user.People, "AddRecord");
//                 mes = $"Select person that {(record.Direction == MoneyDirection.In?"gives money to you": "ownes you money")}:";
//             }
//             else
//             {
//                 //todo default error message
//                 await client.SendTextMessage(user, "InternalError");
//             }
//             await client.SendTextMessage(user, message.Message.MessageId, mes, replyMarkup : keyboard);
//         }
//     }
// }