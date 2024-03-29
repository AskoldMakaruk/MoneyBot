// using System.Linq;
// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
//
// namespace MoneyBot.Telegram.Commands
// {
//     public class EnterTemplateCommand : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             int res = 0;
//             if (user.Status == AccountStatus.EnterTemplate) res += 2;
//             return res;
//         }
//
//         public Task Execute(IClient client)
//         {
//             var values = message.Text.Split('\n').Select(v => v.TrimDoubleSpaces().TrySplit('-', ' '));
//
//             var templates = values.Select(v => new Template()
//             {
//                 Name = v[0],
//                     Sum = v[1].ParseSum(),
//                     Category = user.CurrentTemplate.Category
//             });
//             user.Controller.AddTemplates(templates);
//             user.Status = AccountStatus.Free;
//             await client.SendTextMessage(user, "Template added", Keyboards.MainKeyboard(user));
//         }
//     }
// }