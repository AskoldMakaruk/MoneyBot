using System;

namespace MoneyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new MoneyBot.Telegram.Bot("823973981:AAGYpq1Eyl_AAYGXLeW8s28uCH89S7fsHZA");
            Console.ReadLine();
        }
    }
}
//todo add fastway to guide user to add stuff
//todo conditional keyboards (hide keys that user can't use)