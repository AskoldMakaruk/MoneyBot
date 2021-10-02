using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Clients.ClientExtensions;
using MoneyBot.Controllers;
using MoneyBot.DB.Model;
using MoneyBot.Telegram.Queries;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public class AddCommand : IStaticCommand
    {
        private readonly AccountRepository _accountRepository;

        public AddCommand(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool SuitableFirst(Update update) => update?.Message?.Text == "Add"; //&& account.Status == AccountStatus.Free;

        public async Task Execute(IClient client)
        {
            var update = await client.GetTextMessage();
            var account = _accountRepository.FromMessage(update);

            //todo fast templates and change text on buttons
            var keys = Keyboards.AddType(account);
            if (account.PeopleInited() && account.CategoriesInited())
            {
                await client.SendTextMessage($"This is about", replyMarkup: keys);
            }
            else if (account.PeopleInited())
            {
                //   return AddTypeQuery.SelectRecordType(account, DB.Secondary.RecordType.Transaction);
            }
            else if (account.CategoriesInited())
            {
                // return AddTypeQuery.SelectRecordType(account, DB.Secondary.RecordType.Expense);
            }
            else
            {
                await client.SendTextMessage($"Add category or person first", replyMarkup: keys);
            }
        }
    }
}