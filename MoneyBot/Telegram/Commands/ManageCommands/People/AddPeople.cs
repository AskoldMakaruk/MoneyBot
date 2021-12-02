using System;
using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Services.Extensioins;
using MoneyBot.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = MoneyBot.DB.Model.User;

namespace MoneyBot.Telegram.Commands
{
    public class AddPersonCommand : IStaticCommand
    {
        private readonly User _user;
        private readonly PeopleService _peopleService;

        public AddPersonCommand(User user, PeopleService peopleService)
        {
            _user = user;
            _peopleService = peopleService;
        }

        public bool SuitableFirst(Update update)
        {
            return update.Message?.Text == "Add people";
        }

        public async Task Execute(IClient client)
        {
            var update = await client.GetUpdate();
            await client.SendTextMessage($"Enter new person name:");

            var message = await client.GetTextMessage();
            var name = message.Text;
            var alias = await _peopleService.GetAlias(name);

            if (alias == null)
            {
                await client.SendTextMessage($"I couldn't come up with a **cool alias** for your fren. Please specify what it should be:", parseMode: ParseMode.Markdown);
                alias = (await client.GetTextMessage()).Text;

                while (!await _peopleService.AliasAvailable(alias))
                {
                    await client.SendTextMessage($"Sorry, but u already have a fren with this **alias**.", parseMode: ParseMode.Markdown);
                    alias = (await client.GetTextMessage()).Text;
                }
            }

            alias = alias.ToUpper();

            var data = Guid.NewGuid().ToString()[0..10];
            await client.SendTextMessage($"Confirm adding fren:\n**Name**: {name}\n**Alias**: {alias}",
                replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton()
                {
                    Text = $"Confirm {name}",
                    CallbackData = data
                }),
                parseMode: ParseMode.Markdown);

            var callback = await client.GetUpdateFilter();//.Where(a => a.CallbackQuery?.Data == data);

            await _peopleService.AddPerson(name, alias);

            await client.AnswerCallbackQuery(callback.CallbackQuery.Id, "Fren added");
        }
    }
}