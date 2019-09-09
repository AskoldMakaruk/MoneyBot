namespace MoneyBot.DB.Model
{
    public class Template
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public double Sum { get; set; }
        public string Name { get; set; }
    }
}