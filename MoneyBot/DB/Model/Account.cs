using System.Collections.Generic;

namespace MoneyBot.DB.Model
{
    public class Account
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ChatId { get; set; }

        public ICollection<Fren> Frens { get; set; } = new List<Fren>();
    }

    public class Fren
    {
        public int AccountId { get; set; }
        public int FrenId { get; set; }

        public Account Account { get; set; }
        public Account FrenAccount { get; set; }
    }
}