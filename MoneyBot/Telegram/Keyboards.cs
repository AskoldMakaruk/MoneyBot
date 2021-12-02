using System.Collections.Generic;
using System.Linq;
using MoneyBot.DB.Model;
using MoneyBot.DB.Secondary;
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
        public static ReplyKeyboardMarkup MainKeyboard(User user)
        {

            var firstRow = new List<KeyboardButton>();
            var secondRow = new List<KeyboardButton>();

            if (user.CategoriesInited() || user.PeopleInited())
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

        public static ReplyKeyboardMarkup Manage(User user)
        {
            var firstRow = new List<KeyboardButton>() { "Add categories" };
            var secondRow = new List<KeyboardButton>();
            var thirdRow = new List<KeyboardButton>() { "Add people" };
            var fourthRow = new List<KeyboardButton>();

            if (user.CategoriesInited())
            {
                firstRow.Add("Override categories");
                firstRow.Add("Show categories");

                secondRow.Add("Add templates");
                secondRow.Add("Override templates");
                secondRow.Add("Show templates");
            }
            if (user.PeopleInited())
            {
                thirdRow.Add("Override people");
                thirdRow.Add("Show people");
            }
            // if (user.FundsInited())
            // {
            //     fourthRow.Add("Override funds");
            //     fourthRow.Add("Show funds");
            // }
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

        internal static InlineKeyboardMarkup AddType(User user)
        {
            var keys = new List<InlineKeyboardButton>();
            if (user.PeopleInited())
                keys.Add(new InlineKeyboardButton()
                {
                    CallbackData = "AddType Person",
                        Text = "Someone else and me"
                });
            if (user.CategoriesInited())
                keys.Add(new InlineKeyboardButton()
                {
                    CallbackData = "AddType Category",
                        Text = "Me"
                });
            return keys.ToArray();
        }

        public static InlineKeyboardMarkup CategoryTypes(User user, string query)
        {
            var templatesKeyboard = new List<List<InlineKeyboardButton>>();
            var lastRow = new List<InlineKeyboardButton>
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
            };

            // if (user.CurrentRecord.RecordType == RecordType.Expense)
            // {
            //     var templates = user.Categories.SelectMany(c => c.Templates).ToArray();
            //     if (templates.Length > 0)
            //     {
            //         for (int i = 0; i < templates.Length; i++)
            //         {
            //             var template = templates[i];
            //             var button = new InlineKeyboardButton()
            //             {
            //                 Text = $"{template.Category.Emoji} {template.Name} - {template.Sum}",
            //                 CallbackData = query + " " + template.Id
            //             };
            //             if (templatesKeyboard.Count == 0)
            //             {
            //                 var arr = new List<InlineKeyboardButton>() { button };
            //                 templatesKeyboard.Add(arr);
            //             }
            //             else if (templatesKeyboard.Count > 0)
            //             {
            //                 if (templatesKeyboard.Last().Count == 1)
            //                 {
            //                     templatesKeyboard.Last() [1] = button;
            //                 }
            //                 else
            //                 {
            //                     templatesKeyboard.Add(new List<InlineKeyboardButton> { button });
            //                 }
            //             }
            //         }
            //     }
            // }
            templatesKeyboard.Add(lastRow);
            return templatesKeyboard.ToArray();
        }

        public static InlineKeyboardMarkup Templates(List<Template> templates, string query)
        {
            return templates.Select(t => new InlineKeyboardButton { CallbackData = query + " " + t.Id, Text = t.Category.Emoji + t.Name + ": " + t.Sum }).ToArray();
        }

        public static InlineKeyboardMarkup People(IEnumerable<Person> people, string query)
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

        public static InlineKeyboardMarkup ShowActiveCategories(IEnumerable<ExpenseCategory> input) => Categories(input.Where(c => c.Expenses != null && c.Expenses.Count != 0), "ShowCategory");
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