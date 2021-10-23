// using System.Linq;
// using System.Threading.Tasks;
// using BotFramework.Abstractions;
// using BotFramework.Clients.ClientExtensions;
// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
//
// namespace MoneyBot.Telegram.Commands
// {
//     public class OverridePeopleCommand : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             return (update.Message?.Text == "Override people");
//         }
//
//         public async Task Execute(IClient client)
//         {
//             await client.SendTextMessage("This will override your people and delete attached transactions.\nEnter new people in format:\n" +
//                                          "[alias] [name]", replyMarkup: Keyboards.Cancel);
//             
//             var values = message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));
//             var people = values.Select(v => new Person()
//             {
//                 Account = account,
//                 Alias = v[0],
//                 Name = v[1]
//             });
//             //categories to be saved
//             var saved = account.People.Where(c => people.FirstOrDefault(e => e.Name == c.Name && e.Alias == c.Alias) != null);
//
//             //to save records about people that haven't changed
//             account.People = saved.Union(people.Where(c => saved.FirstOrDefault(e => e.Name == c.Name && e.Alias == c.Alias) == null)).ToList();
//
//             account.Status = AccountStatus.Free;
//             await client.SendTextMessage(account, "People added", replyMarkup: Keyboards.MainKeyboard(account));
//         }
//     }
// }