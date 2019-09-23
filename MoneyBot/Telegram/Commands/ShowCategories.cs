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
        public override OutMessage Execute()
        {
            if (Message.Text == "Show categories")
            {
                return ToCategory(Account, Client);

            }
            if (Message.Text == "Show people")
            {
                return ToPeople(Account, Client);

            }
            return Relieve();
        }
        public static OutMessage ToCategory(Account Account, Bot Client)
        {
            return new OutMessage(Account, $"You have {Account.Categories.Count} categories.", replyMarkup : Keyboards.Categories(Account.Categories, "ShowCategory"));
        }
        public static OutMessage ToPeople(Account Account, Bot Client)
        {
            return new OutMessage(Account, $"You have {Account.People.Count} people.", replyMarkup : Keyboards.People(Account.People, "ShowPeople"));
        }
    }
}