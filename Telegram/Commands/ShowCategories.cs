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
                if (Account.Categories != null)
                    await Client.SendTextMessageAsync(Account.ChatId, $"You have {Account.Categories.Count} categories.", replyMarkup : Keyboards.Categories(Account.Categories, "ShowCategory"));
                else await Client.SendTextMessageAsync(Account.ChatId, $"You have 0 categories", replyMarkup : Keyboards.Main);
                return;
            }
            if (Message.Text == "Show people")
            {
                if (Account.People != null)
                    await Client.SendTextMessageAsync(Account.ChatId, $"You have {Account.People.Count} people.", replyMarkup : Keyboards.People(Account.People, "ShowPeople"));
                else await Client.SendTextMessageAsync(Account.ChatId, $"You have 0 people", replyMarkup : Keyboards.Main);
                return;
            }
            Account.Status = AccountStatus.Free;
            Relieve();
        }
    }
}