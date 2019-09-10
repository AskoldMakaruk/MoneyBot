using System;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterExpenseCommand : Command
    {
        public EnterExpenseCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.EnterExpenseSum) res += 2;
            return res;
        }
        public override async void Execute()
        {
            var values = Message.Text.TrySplit(new [] { '-', ' ' });
            var sum = values[1].ParseSum();
            if (sum != -1)
            {
                Account.CurrentExpense.Description = values[0];
                Account.CurrentExpense.Sum = sum;
                Account.CurrentExpense.Date = DateTime.Now;
                Controller.AddExpense(Account.CurrentExpense);
                await Client.SendTextMessageAsync(Account.ChatId, $"Success!", replyMarkup : Keyboards.Main);
                Account.Status = AccountStatus.Free;
                return;
            }
            await Client.SendTextMessageAsync(Account.ChatId, $"Sum cannot be parsed", replyMarkup : Keyboards.Cancel);
        }
        public override async void Relieve()
        {
            await Client.SendTextMessageAsync(Account.ChatId, $"You shall be freed", replyMarkup : Keyboards.Main);
            Account.Status = AccountStatus.Free;
            return;
        }
    }
}