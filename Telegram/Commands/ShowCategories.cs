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
        public override void Execute()
        {
            if (Message.Text == "Show categories")
            {
                ToCategory(Account, Client);
                return;
            }
            if (Message.Text == "Show people")
            {
                ToPeople(Account, Client);
                return;
            }
            Relieve();
        }
        public static async void ToCategory(Account Account, Bot Client)
        {
            await Client.SendTextMessageAsync(Account, $"You have {Account.Categories.Count} categories.", replyMarkup : Keyboards.Categories(Account.Categories, "ShowCategory"));
        }
        public static async void ToPeople(Account Account, Bot Client)
        {
            await Client.SendTextMessageAsync(Account, $"You have {Account.People.Count} people.", replyMarkup : Keyboards.People(Account.People, "ShowPeople"));
        }
    }
}