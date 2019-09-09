using System;

namespace MoneyBot.DB.Model
{
    public class Exspense
    {
        public int Id { get; set; }
        public int ExspenseId { get; set; }
        public int AccountId { get; set; }
        public double Sum { get; set; }
        public DateTime Date { get; set; }
    }
}