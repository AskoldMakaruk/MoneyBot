namespace MoneyBot.DB.Model
{
    public class Fund
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public double TotalGoal { get; set; }
        public int LenghtMonths { get; set; }

    }
}