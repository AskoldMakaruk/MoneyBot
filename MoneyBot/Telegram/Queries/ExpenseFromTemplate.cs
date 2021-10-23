// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
// namespace MoneyBot.Telegram.Queries
// {
//     public class ExepenseFromTemplateQuery : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             return message.Data.StartsWith("Template");
//         }
//         public async Task Execute(IClient client)
//         {
//             if (message.Data.TryParseId(out var template))
//             {
//                 Controller.AddExpense(template);
//                 account.Status = AccountStatus.Free;
//                 await client.SendTextMessage(account, "Success!") { EditMessageId = message.Message.MessageId };
//             }
//             else await client.SendTextMessage(message.Id, "Server error: template not found");
//
//         }
//     }
// }