using System;
using System.Globalization;
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

        public static bool IsNullOrEmpty(this string source) => string.IsNullOrEmpty(source);

        public static string TrimDoubleSpaces(this string source)
        {
            Regex regex = new Regex(@"[\s]{2,}", RegexOptions.None);
            return regex.Replace(source, " ");
        }

        public static bool TryParseId(this string source, out int result)
        {
            return int.TryParse(source.Substring(source.IndexOf(" ", StringComparison.Ordinal) + 1), out result);
        }

        public static double ParseSum(this string source)
        {
            return double.TryParse(source.Trim().Replace(',','.'), NumberStyles.AllowDecimalPoint,
                       CultureInfo.InvariantCulture, out var sum)
                   ? sum
                   : -1;
        }

        public static double CountSum(this Person source)
        {
            return source.Transactions.Select(t => t.Type == MoneyDirection.In ? t.Sum : -t.Sum).Sum();
        }

        public static bool CategoriesInited(this User user)
        {
            return user.Categories != null && user.Categories.Count != 0;
        }

        public static bool CategoriesInitedAndNotEmpty(this User user)
        {
            return CategoriesInited(user) &&
                   user.Categories.Where(c => c.Expenses != null).SelectMany(c => c.Expenses).Count() != 0;
        }

        public static bool PeopleInited(this User user)
        {
            return user.Frens != null && user.Frens.Count != 0;
        }
        
        public static bool PeopleInitedAndNotEmpty(this User user)
        {
            return PeopleInited(user) &&
                   user.Frens.Where(c => c.Transactions != null).SelectMany(c => c.Transactions).Count() != 0;
        }
        
        // public static bool FundsInited(this User user)
        // {
        //     return user.Funds != null && user.Funds.Count != 0;
        // }
    }
}