using System.Linq;
using System.Text.RegularExpressions;
using MoneyBot.DB.Model;

namespace MoneyBot
{
    public static class Helper
    {
        public static string[] TrySplit(this string source, params char[] splitChars)
        {
            return source.Split(splitChars.First(c => source.Contains(c)));
        }
        public static string TrimDoubleSpaces(this string source)
        {
            Regex regex = new Regex(@"[\s]{2,}", RegexOptions.None);
            return regex.Replace(source, " ");
        }
        public static bool TryParseId(this string source, out int result)
        {
            return int.TryParse(source.Substring(source.IndexOf(" ") + 1), out result);
        }
        public static double ParseSum(this string source)
        {
            return double.TryParse(source.Trim().Replace('.', ','), out var sum) ? sum : -1;
        }
        public static double CountSum(this Person source)
        {
            return source.Transactions.Select(t => t.Type == MoneyDirection.In? t.Sum: -t.Sum).Sum();
        }
        public static bool CategoriesInited(this Account account)
        {
            return account.Categories != null && account.Categories.Count != 0;
        }
        public static bool PeopleInited(this Account account)
        {
            return account.People != null && account.People.Count != 0;
        }
        public static bool FundsInited(this Account account)
        {
            return account.Funds != null && account.Funds.Count != 0;
        }
    }
}