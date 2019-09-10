namespace MoneyBot.DB.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public Person Person { get; set; }
        public double Sum { get; set; }
    }
}