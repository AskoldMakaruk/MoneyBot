using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class ShowCategoriesCommand : Command
    {
        public ShowCategoriesCommand(Message message, Bot Client, Account Account) : base(message, Client, Account) { }
        public override int Suitability()
        {
            int res = 0;
            if (Account.Status == AccountStatus.ChooseShow) res++;
            return res;
        }
        public override async void Execute()
        {
            if (Message.Text == "Show categories")
            {
                await Client.SendTextMessageAsync(Account.ChatId, $"You have {Account.Categories.Count} categories.", replyMarkup : Keyboards.Categories(Account.Categories, "ShowCategory"));
                return;
            }
            if (Message.Text == "Show people")
            {
                await Client.SendTextMessageAsync(Account.ChatId, $"You have {Account.People.Count} people.", replyMarkup : Keyboards.People(Account.People, "ShowPeople"));
                return;
            }
            Account.Status = AccountStatus.Free;
            Relieve();
        }
    }
}