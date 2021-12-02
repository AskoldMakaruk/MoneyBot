// using System.Linq;
// using System.Text.RegularExpressions;
// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
//
// namespace MoneyBot.Telegram.Commands
// {
//     public class AddCategoryCommand : IStaticCommand
//     {
//         public AddCategoryCommand() : base() { }
//
//         public bool SuitableFirst(Update update)
//         {
//             int res = 0;
//             if (user.Status == AccountStatus.AddCategories) res += 2;
//             return res;
//         }
//         public Task Execute(IClient client)
//         {
//             var regex = new Regex(@"\w{0,} - .{2,3} - \w{0,}");
//             var values = message.Text.Split('\n').Where(v => regex.IsMatch(v)).Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));
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
//             user.Controller.AddCategories(categories);
//             user.Status = AccountStatus.Free;
//             var mes = "";
//             if (categories.Count() == 0)
//             {
//                 mes = "No categories were added.";
//             }
//             else if (categories.Count() == 1)
//             {
//                 mes = $"Category {categories.First().Name} was added.";
//             }
//             else
//             {
//                 mes = $"{categories.Count()} categories were added.";
//             }
//             await client.SendTextMessage(user, mes, Keyboards.MainKeyboard(user));
//         }
//     }
// }