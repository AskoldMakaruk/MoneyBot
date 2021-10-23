// using System.Linq;
// using System.Text;
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
//     public class SelectRecordFromIdQuery : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             return update.CallbackQuery?.Data.StartsWith("AddRecord") ?? false;
//         }
//
//         public async Task Execute(IClient client)
//         {
//             var message = (await client.GetUpdate()).CallbackQuery;
//             var record = account.CurrentRecord;
//             if (record == null) await client.EditMessageText(message.Id, "You have no expenses");
//
//             if (!message.Data.TryParseId(out var id))
//             {
//                 await client.SendTextMessage(message.Id, "Internal error");
//             }
//
//             record.FromId = id;
//             account.Status = AccountStatus.EnterRecordSum;
//
//             string enterDetails = "Enter details in format:\n[Description] - [sum]\nOr\n[sum]";
//
//             var builder = new StringBuilder();
//             InlineKeyboardMarkup replyMarkup = null;
//             if (record.RecordType == RecordType.Expense)
//             {
//                 var category = account.Categories.First(c => c.Id == id);
//                 var templates = category.Templates;
//                 builder.AppendLine($"Adding expense to {category.ToString()}");
//                 builder.AppendLine(enterDetails);
//                 builder.AppendLine($"Example:\nPork - 229.33\n39.51");
//
//                 if (templates != null && templates.Count > 0)
//                 {
//                     builder.AppendLine("Or use template:");
//                     replyMarkup = Keyboards.Templates(templates, "Template");
//                 }
//             }
//             else if (record.RecordType == RecordType.Transaction)
//             {
//                 var person = account.People.First(p => p.Id == id);
//                 builder.AppendLine($"Adding transaction between you and{person.Name}");
//                 builder.AppendLine(enterDetails);
//                 builder.AppendLine("Example:\nFor vodka - 50\n49");
//             }
//             else await client.SendTextMessage(message.Id, "Internal error");
//
//             await client.SendTextMessage(account, builder.ToString(), replyMarkup)
//             {
//                 EditMessageId = message.Message.MessageId
//             }
//             ;
//         }
//     }
// }