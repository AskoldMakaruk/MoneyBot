using System.Collections.Generic;

namespace MoneyBot.DB.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ChatId { get; set; }

        public ICollection<Person> Frens { get; set; } = new List<Person>();
        public ICollection<ExpenseCategory> Categories { get; set; } = new List<ExpenseCategory>();
    }    
}

