using System;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterTransactionCommand : Command
    {
        public EnterTransactionCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.EnterTransactionSum) res++;
            return res;
        }
        public override async void Execute()
        {
            var values = Message.Text.TrySplit('-', ' ');
            var sum = values[1].ParseSum();
            if (sum != -1)
            {
                Account.CurrentTransaction.Description = values[0];
                Account.CurrentTransaction.Sum = sum;
                Account.CurrentTransaction.Date = DateTime.Now;
                Controller.AddTransaction(Account.CurrentTransaction);
                await Client.SendTextMessageAsync(Account.ChatId, $"Success!", replyMarkup : Keyboards.Main);
                Account.Status = AccountStatus.Free;
                return;
            }
            await Client.SendTextMessageAsync(Account.ChatId, $"Sum cannot be parsed", replyMarkup : Keyboards.Cancel);
        }
    }
}