// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
// namespace MoneyBot.Telegram.Commands
// {
//     public class TemplateCommand : IStaticCommand
//     {
//         public TemplateCommand() : base() { }
//         public bool SuitableFirst(Update update)
//         {
//             int res = 0;
//             if (message.Text.ToLower().Contains("templat") && account.Status == AccountStatus.Manage) res += 2;
//             return res;
//         }
//         public Task Execute(IClient client)
//         {
//             if (message.Text == "Add templates")
//             {
//                 account.CurrentTemplate = new Template();
//                 await client.SendTextMessage(account, "Select category for new template:", replyMarkup : Keyboards.Categories(account.Categories, "AddTemplate"));
//             }
//             return Relieve(message, account);
//         }
//     }
// }