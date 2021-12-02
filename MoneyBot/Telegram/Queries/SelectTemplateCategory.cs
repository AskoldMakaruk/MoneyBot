// using System.Linq;
// using System.Threading.Tasks;
// using BotFramework.Abstractions;
// using BotFramework.Clients.ClientExtensions;
// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
//
// namespace MoneyBot.Telegram.Queries
// {
//     public class SelectTemplateCategoryQuery : IStaticCommand
//     {
//
//         public bool SuitableFirst(Update update)
//         {
//             return message.Data.StartsWith("AddTemplate");
//         }
//         public async Task Execute(IClient client)
//         {
//             if (!message.Data.TryParseId(out var categoryId))
//             {
//                 await client.SendTextMessage(message.Id, "Internal error");
//
//             }
//             user.CurrentTemplate.Category = user.Categories.First(c => c.Id == categoryId);
//             var templates = user.CurrentTemplate.Category.Templates;
//             user.Status = AccountStatus.EnterTemplate;
//             await client.SendTextMessage(
//                 user,
//                 message.Message.MessageId,
//                 $"Adding template to {user.CurrentTemplate.Category.ToString()}\n" +
//                 $@"Enter new template in format:
// [Name] - [sum]
//
// Example:
// Pork - 229.33");
//
//         }
//     }
// }