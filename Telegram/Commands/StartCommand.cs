using MoneyBot.DB.Model;
using StickerMemeBot.Telegram.Commands;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class StartCommand : Command
    {
        public StartCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }

        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.Start) res += 2;
            if (Message.Text.StartsWith("/start")) res += 2;
            return res;
        }
        public override async void Execute()
        {
            await Client.SendTextMessageAsync(Account.ChatId, "Welcome to MoneyBot.", replyMarkup : Keyboards.Main);
            Account.Status = AccountStatus.Free;
        }
    }
}