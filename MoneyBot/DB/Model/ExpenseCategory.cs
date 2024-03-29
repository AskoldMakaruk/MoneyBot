using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public long UserId { get; set; }

        public User User { get; set; }

        public string Name { get; set; }
        public string Emoji { get; set; }
        public MoneyDirection Type { get; set; }

        public List<Expense> Expenses { get; set; }
        public List<Template> Templates { get; set; }

        public override string ToString() => $"{Emoji} {Name}";
    }

    public enum MoneyDirection : byte
    {
        Out,
        In
    }
}