// using System;
// using System.Linq;
// using MoneyBot.DB.Model;
// using MoneyBot.DB.Secondary;
// using Telegram.Bot.Types;
// namespace MoneyBot.Telegram.Commands
// {
//     public class EnterRecordSumCommand : IStaticCommand
//     {
//         public bool SuitableFirst(Update update)
//         {
//             int res = 0;
//             if (account.Status == AccountStatus.EnterRecordSum) res += 2;
//             return res;
//         }
//         public Task Execute(IClient client)
//         {
//             var text = message.Text;
//
//             var sum = -1.0;
//             var description = "";
//
//             var success = false;
//
//             if (text.Contains('-'))
//             {
//                 var values = message.Text.TrySplit('-');
//                 if (values.Length == 2)
//                 {
//                     success = true;
//                     description = values[0];
//                     sum = values[1].ParseSum();
//                 }
//             }
//             else
//             {
//                 success = double.TryParse(text.Trim(), out sum);
//             }
//
//             if (success && account.CurrentRecord != null)
//             {
//                 var record = account.CurrentRecord;
//                 record.Description = description;
//                 record.Sum = sum;
//
//                 record.Date = DateTime.Now;
//                 switch (record.RecordType)
//                 {
//                     case RecordType.Expense:
//                         account.Controller.AddExpense(new Expense
//                         {
//                             Category = account.Categories.First(c => c.Id == record.FromId),
//                                 Date = record.Date,
//                                 Description = record.Description,
//                                 Sum = record.Sum
//                         });
//                         break;
//                     case RecordType.Transaction:
//                         account.Controller.AddTransaction(new Transaction
//                         {
//                             Person = account.People.First(c => c.Id == record.FromId),
//                                 Date = record.Date,
//                                 Description = record.Description,
//                                 Sum = record.Sum,
//                                 Type = record.Direction
//                         });
//                         break;
//                 }
//
//                 account.Status = AccountStatus.Free;
//                 await client.SendTextMessage(account, $"Success!", replyMarkup : Keyboards.MainKeyboard(account));
//
//             }
//             await client.SendTextMessage(account, $"Sum cannot be parsed", replyMarkup : Keyboards.Cancel);
//         }
//         public override Response Relieve(Message message, Account account)
//         {
//             account.Status = AccountStatus.Free;
//             await client.SendTextMessage(account, $"You shall be freed", replyMarkup : Keyboards.MainKeyboard(account));
//         }
//     }
// }