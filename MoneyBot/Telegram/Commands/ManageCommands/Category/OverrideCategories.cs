// using System.Linq;
// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
// namespace MoneyBot.Telegram.Commands
// {
//     public class OverrideCategoriesCommand : IStaticCommand
//     {
//         public OverrideCategoriesCommand() : base() { }
//         public bool SuitableFirst(Update update)
//         {
//             int res = 0;
//             if (user.Status == AccountStatus.OverrideCategories) res += 2;
//             return res;
//         }
//         public Task Execute(IClient client)
//         {
//             var values = message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));
//
//             var categories = values.Select(v => new ExpenseCategory()
//             {
//                 User = user,
//                     Emoji = v[0],
//                     //TODO default type if one is missing
//                     Type = v[1].ToLower().Contains("in") ? MoneyDirection.In : MoneyDirection.Out,
//                     Name = v[2],
//
//             });
//             //categories to be saved
//             var saved = user.Categories.Where(c => categories.FirstOrDefault(e => e.Name == c.Name && e.Emoji == c.Emoji) != null);
//             //to save records about categories that haven't changed
//             user.Categories = saved.Union(categories.Where(c => saved.FirstOrDefault(e => e.Name == c.Name && e.Emoji == c.Emoji) == null)).ToList();
//
//             user.Controller.SaveChanges();
//             user.Status = AccountStatus.Free;
//             await client.SendTextMessage(user, "Categories overrided", Keyboards.MainKeyboard(user));
//         }
//     }
// }