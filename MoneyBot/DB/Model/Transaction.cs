using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class Transaction
    {
        public int Id { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public double Sum { get; set; }
        public MoneyDirection Type { get; set; }
    }
}