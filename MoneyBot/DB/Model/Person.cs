using System.Collections.Generic;

namespace MoneyBot.DB.Model
{
    public class Person
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        
        public User User { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}