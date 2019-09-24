using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class ShowCategoriesCommand : Command
    {
        public ShowCategoriesCommand() : base() { }
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.ChooseShow) res++;
            return res;
        }
        public override Response Execute(Message message, Account account)
        {
            if (message.Text == "Show categories")
            {
                return ToCategory(account);

            }
            if (message.Text == "Show people")
            {
                return ToPeople(account);

            }
            return Relieve(message, account);
        }
        public static Response ToCategory(Account account)
        {
            return new Response(account, $"You have {account.Categories.Count} categories.", replyMarkup : Keyboards.Categories(account.Categories, "ShowCategory"));
        }
        public static Response ToPeople(Account account)
        {
            return new Response(account, $"You have {account.People.Count} people.", replyMarkup : Keyboards.People(account.People, "ShowPeople"));
        }
    }
}