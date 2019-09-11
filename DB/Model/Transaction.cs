using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class Transaction
    {
        public int Id { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public double Sum { get; set; }
    }
}