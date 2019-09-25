using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class ShowCategoriesCommand : Command
    {
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (account.Status == AccountStatus.ChooseShow) res += 2;
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
            return new Response(account, $"Select one to see info.", replyMarkup : Keyboards.ShowActiveCategories(account.Categories));
        }
        public static Response ToPeople(Account account)
        {
            return new Response(account, $"Select one to see info.", replyMarkup : Keyboards.People(account.People, "ShowPeople"));
        }
    }
}