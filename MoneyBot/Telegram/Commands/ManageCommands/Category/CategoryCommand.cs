// using System.Linq;
// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
//
// namespace MoneyBot.Telegram.Commands
// {
//     public class CategoryCommand : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             int res = 0;
//             if (user.Status == AccountStatus.Manage && message.Text.Contains("categor")) res += 2;
//             return res;
//         }
//         public Task Execute(IClient client)
//         {
//             if (message.Text == "Add categories")
//             {
//                 user.Status = AccountStatus.AddCategories;
//                 await client.SendTextMessage(user, "Enter new categories in format:\n[emoji] - [categoryType(In/Out)] - [category name]\n\nExample:\n💊 - in - Hard drugs\n🥦 - out - Trees\n👨🏿 - in - Nigga", Keyboards.Cancel);
//             }
//             if (message.Text == "Show categories")
//             {
//                 if (user.Categories != null && user.Categories.Count != 0)
//                     await client.SendTextMessage(user, $"{string.Join("\n", user.Categories.Select(c => $"{c.Emoji} - {c.Type} - {c.Name}"))}");
//                 else await client.SendTextMessage(user, $"You have no categories.");
//             }
//             if (message.Text == "Override categories")
//             {
//                 user.Status = AccountStatus.OverrideCategories;
//                 await client.SendTextMessage(user, "This will override your categories and delete attached expenses.\nEnter new categories in format:\n[emoji] - [categoryType(In/Out)] - [category name]\n\nExample:\n💊 - in - Hard drugs\n🥦 - out - Trees\n👨🏿 - in - Nigga", replyMarkup : Keyboards.Cancel);
//             }
//             return Relieve(message, user);
//         }
//     }
// }