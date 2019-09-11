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
                new KeyboardButton("Add expense"),
                    new KeyboardButton("Show categories")

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
                new KeyboardButton("Add category"),
                    new KeyboardButton("Edit category"),
                    new KeyboardButton("Show categories")
            },
            new []
            {
                new KeyboardButton("Add template"),
                    new KeyboardButton("Edit template"),
                    new KeyboardButton("Show templates")
            },
            new []
            {
                new KeyboardButton("Add person"),
                    new KeyboardButton("Edit person"),
                    new KeyboardButton("Show persons")
            },
            new []
            {
                new KeyboardButton("Add fund"),
                    new KeyboardButton("Edit fund"),
                    new KeyboardButton("Show funds")
            }

        }, true);

        public static IReplyMarkup CategoryTypes => new InlineKeyboardMarkup(new InlineKeyboardButton[]
        {
            new InlineKeyboardButton()
                {
                    CallbackData = "ExpenseType Out",
                        Text = "😳😭I'm paying😤"
                },
                new InlineKeyboardButton()
                {
                    CallbackData = "ExpenseType In",
                        Text = "😎👌💵I'm being payed💵👌"
                },
        });

        public static InlineKeyboardMarkup Templates(List<Template> templates, string query)
        {
            return templates.Select(t => new InlineKeyboardButton { CallbackData = query + " " + t.Id, Text = t.Category.Emoji + t.Name + ": " + t.Sum }).ToArray();
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