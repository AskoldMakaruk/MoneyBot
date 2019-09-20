using System;
using System.Collections.Generic;
using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types.ReplyMarkups;
namespace MoneyBot.Telegram
{
    public static class Keyboards
    {
        public static IReplyMarkup Cancel => new ReplyKeyboardMarkup(new []
            {
                new KeyboardButton("Cancel")
            },
            true, true);
        public static IReplyMarkup Clear => new ReplyKeyboardRemove();
        public static ReplyKeyboardMarkup MainKeyboard(Account account)
        {

            var firstRow = new List<KeyboardButton>();
            var secondRow = new List<KeyboardButton>();

            if (account.CategoriesInited() || account.PeopleInited())
            {
                firstRow = new List<KeyboardButton>
                {
                    "Add",
                    "Show"
                };
                secondRow.Add("Stats");
            }
            secondRow.Add("Manage Menu");
            return new ReplyKeyboardMarkup(new []
            {
                firstRow,
                secondRow
            }, true);
        }

        public static ReplyKeyboardMarkup Manage(Account account)
        {
            var firstRow = new List<KeyboardButton>() { "Add categories" };
            var secondRow = new List<KeyboardButton>();
            var thirdRow = new List<KeyboardButton>() { "Add people" };
            var fourthRow = new List<KeyboardButton>();

            if (account.CategoriesInited())
            {
                firstRow.Add("Override categories");
                firstRow.Add("Show categories");

                secondRow.Add("Add templates");
                secondRow.Add("Override templates");
                secondRow.Add("Show templates");
            }
            if (account.PeopleInited())
            {
                thirdRow.Add("Override people");
                thirdRow.Add("Show people");
            }
            if (account.FundsInited())
            {
                fourthRow.Add("Override funds");
                fourthRow.Add("Show funds");
            }
            return new ReplyKeyboardMarkup(new []
            {
                firstRow,
                secondRow,
                thirdRow,
                fourthRow
            }, true);
        }

        public static ReplyKeyboardMarkup MainShow => new ReplyKeyboardMarkup(
            new KeyboardButton[]
            {
                "Show people",
                "Show categories"
            },
            true);

        internal static InlineKeyboardMarkup AddType(Account account)
        {
            var keys = new List<InlineKeyboardButton>();
            if (account.PeopleInited())
                keys.Add(new InlineKeyboardButton()
                {
                    CallbackData = "AddType Person",
                        Text = "Someone else and me"
                });
            if (account.CategoriesInited())
                keys.Add(new InlineKeyboardButton()
                {
                    CallbackData = "AddType Category",
                        Text = "Me"
                });
            return keys.ToArray();
        }

        public static InlineKeyboardMarkup CategoryTypes(string query) => new InlineKeyboardMarkup(new InlineKeyboardButton[]
        {
            new InlineKeyboardButton()
                {
                    CallbackData = $"{query} Out",
                        Text = "😳😭I'm paying😤"
                },
                new InlineKeyboardButton()
                {
                    CallbackData = $"{query} In",
                        Text = "😎👌💵I'm being payed💵👌"
                },
        });

        //TODO page navigation with buttons

        public static InlineKeyboardMarkup Templates(List<Template> templates, string query)
        {
            return templates.Select(t => new InlineKeyboardButton { CallbackData = query + " " + t.Id, Text = t.Category.Emoji + t.Name + ": " + t.Sum }).ToArray();
        }

        internal static InlineKeyboardMarkup People(IEnumerable<Person> people, string query)
        {
            var categories = people.ToArray();
            var keys = new List<List<InlineKeyboardButton>>();

            for (int i = 0; i < categories.Length; i++)
            {
                var category = categories[i];
                var button =
                    new InlineKeyboardButton()
                    {
                        Text = $"{category.Name}",
                        CallbackData = query + " " + category.Id
                    };
                if (keys.Count == 0)
                {
                    keys.Add(new List<InlineKeyboardButton> { button });
                }
                else if (keys.Count > 0)
                {
                    if (keys.Last().Count == 1)
                    {
                        keys.Last().Add(button);
                    }
                    else
                    {
                        keys.Add(new List<InlineKeyboardButton> { button });
                    }
                }
            }
            return keys.ToArray();
        }

        public static InlineKeyboardMarkup Categories(IEnumerable<ExpenseCategory> input, string query)
        {
            var categories = input.ToArray();
            var keys = new List<List<InlineKeyboardButton>>();

            for (int i = 0; i < categories.Length; i++)
            {
                var category = categories[i];
                var button =
                    new InlineKeyboardButton()
                    {
                        Text = $"{category.Emoji} {category.Name}",
                        CallbackData = query + " " + category.Id
                    };
                if (keys.Count == 0)
                {
                    keys.Add(new List<InlineKeyboardButton> { button });
                }
                else if (keys.Count > 0)
                {
                    if (keys.Last().Count == 1)
                    {
                        keys.Last().Add(button);
                    }
                    else
                    {
                        keys.Add(new List<InlineKeyboardButton> { button });
                    }
                }
            }
            return keys.ToArray();
        }
    }
}