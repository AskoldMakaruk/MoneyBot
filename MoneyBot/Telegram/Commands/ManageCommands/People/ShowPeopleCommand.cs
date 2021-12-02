using System.Linq;
using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Services.Extensioins;
using Telegram.Bot.Types;
using User = MoneyBot.DB.Model.User;

namespace MoneyBot.Telegram.Commands
{
    public class ShowPeopleCommand : IStaticCommand
    {
        private readonly User _user;

        public ShowPeopleCommand(User user)
        {
            _user = user;
        }

        public bool SuitableFirst(Update update) => update.Message?.Text == "Show people";

        public async Task Execute(IClient client)
        {
            if (_user.Frens != null && _user.Frens.Count != 0)
            {
                await client.SendTextMessage($"{string.Join("\n", _user.Frens.Select(c => $"{c.Alias} - {c.Name}"))}");
            }
            else
            {
                await client.SendTextMessage($"You have no frens.");
            }
        }
    }
}