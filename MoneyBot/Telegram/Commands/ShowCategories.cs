// using System.Threading.Tasks;
// using BotFramework.Abstractions;
// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
//
// namespace MoneyBot.Telegram.Commands
// {
//     public class ShowCategoriesCommand : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             int res = 0;
//             if (user.Status == AccountStatus.ChooseShow) res += 2;
//             return res;
//         }
//         public Task Execute(IClient client)
//         {
//             if (message.Text == "Show categories")
//             {
//                 return ToCategory(user);
//
//             }
//             if (message.Text == "Show people")
//             {
//                 return ToPeople(user);
//
//             }
//             return Relieve(message, user);
//         }
//        
//     }
// }