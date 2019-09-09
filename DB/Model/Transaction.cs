namespace MoneyBot.DB.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int PersonId { get; set; }
        public double Sum { get; set; }
    }
}