using System;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;
namespace MoneyBot.Telegram.Commands
{
    public class EnterExspenseSumCommand : Command
    {
        public EnterExspenseSumCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.EnterExspenseSum) res += 2;
            if (Message.Text != null) res++;
            return res;
        }
        public override async void Execute()
        {
            if (double.TryParse(Message.Text.Replace('.', ','), out var sum))
            {
                Account.CurrentExspense.Sum = sum;
                Account.CurrentExspense.Date = DateTime.Now;
                Controller.AddExspense(Account.CurrentExspense);
                await Client.SendTextMessageAsync(Account.ChatId, $"Succses!", replyMarkup : Keyboards.Main);
                Account.Status = AccountStatus.Free;
                return;
            }
            if (Message.Text == "Cancel")
            {
                await Client.SendTextMessageAsync(Account.ChatId, $"You shall be freed", replyMarkup : Keyboards.Main);
                Account.Status = AccountStatus.Free;
                return;
            }
            await Client.SendTextMessageAsync(Account.ChatId, $"Sum cannot be parsed", replyMarkup : Keyboards.Cancel);
        }
    }
}