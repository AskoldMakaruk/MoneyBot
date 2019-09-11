using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}