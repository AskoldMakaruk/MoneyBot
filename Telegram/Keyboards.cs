using System.Collections.Generic;
using System.Linq;
using MoneyBot.DB.Model;
using Telegram.Bot.Types.ReplyMarkups;
namespace MoneyBot.Telegram
{
    public static class Keyboards
    {
        public static ReplyKeyboardMarkup Main => new ReplyKeyboardMarkup(new []
        {
            new []
            {
                new KeyboardButton("Add exspense"),
                    new KeyboardButton("Stats")
            },
            new []
            {
                new KeyboardButton("Show categories"),
                    new KeyboardButton("Another menu?")
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
        public static InlineKeyboardMarkup Categories(ExspenseCategory[] categories, string query)
        {
            var keys = new List<List<InlineKeyboardButton>>();

            for (int i = 0; i < categories.Length; i++)
            {
                var category = categories[i];
                var button =
                    new InlineKeyboardButton()
                    {
                        Text = $"{category.Emoji} {category.Name}",
                        CallbackData = query + category.Id
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