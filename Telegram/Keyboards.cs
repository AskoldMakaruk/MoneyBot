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
        public static ReplyKeyboardMarkup Main => new ReplyKeyboardMarkup(new []
        {
            new []
            {
                new KeyboardButton("Add"),
                    new KeyboardButton("Show")

            },
            new []
            {

                new KeyboardButton("Stats"),
                    new KeyboardButton("Manage Menu")

            }

        }, true);

        public static ReplyKeyboardMarkup Manage => new ReplyKeyboardMarkup(new []
        {
            new []
            {
                new KeyboardButton("Add categories"),
                    new KeyboardButton("Override category"),
                    new KeyboardButton("Show categories")
            },
            new []
            {
                new KeyboardButton("Add templates"),
                    new KeyboardButton("Override templates"),
                    new KeyboardButton("Show templates")
            },
            new []
            {
                new KeyboardButton("Add people"),
                    new KeyboardButton("Override people"),
                    new KeyboardButton("Show people")
            },
            new []
            {
                new KeyboardButton("Add fund"),
                    new KeyboardButton("Override funds"),
                    new KeyboardButton("Show funds")
            }
        }, true);

        public static ReplyKeyboardMarkup MainShow => new ReplyKeyboardMarkup(new []
        {
            new []
            {
                new KeyboardButton("Show people"),
                    new KeyboardButton("Show categories")
            }
        }, true);

        internal static InlineKeyboardMarkup AddType(Account account)
        {
            var keys = new List<InlineKeyboardButton>();
            if (account.People?.Count > 0)
                keys.Add(new InlineKeyboardButton()
                {
                    CallbackData = "AddType Person",
                        Text = "Someone else and me"
                });
            if (account.Categories.Count > 0)
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

        //todo page navigation with buttons

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