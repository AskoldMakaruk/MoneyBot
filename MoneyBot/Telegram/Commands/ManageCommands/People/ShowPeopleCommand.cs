using System.Linq;
using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Clients.ClientExtensions;
using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class ShowPeopleCommand : IStaticCommand
    {
        private readonly Account _account;

        public ShowPeopleCommand(Account account)
        {
            _account = account;
        }

        public bool SuitableFirst(Update update) => update.Message?.Text == "Show people";

        public async Task Execute(IClient client)
        {
            if (_account.People != null && _account.People.Count != 0)
            {
                await client.SendTextMessage($"{string.Join("\n", _account.People.Select(c => $"{c.Alias} - {c.Name}"))}");
            }
            else
            {
                await client.SendTextMessage($"You have no frens.");
            }
        }
    }
}