// using MoneyBot.DB.Model;
// using Telegram.Bot.Types;
// namespace MoneyBot.Telegram.Commands
// {
//     public class MangeCommand : IStaticCommand
//     {
//         public MangeCommand() : base() { }
//         public bool SuitableFirst(Update update)
//         {
//             int res = 0;
//             if (user.Status == AccountStatus.Manage) res++;
//             return res;
//         }
//         public Task Execute(IClient client)
//         {
//             return Relieve(message, user);
//         }
//     }
// }