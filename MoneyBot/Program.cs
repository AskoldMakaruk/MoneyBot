using System;

namespace MoneyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new MoneyBot.Telegram.Bot("823973981:AAGYpq1Eyl_AAYGXLeW8s28uCH89S7fsHZA");
            bot.StartReceiving();
            Console.ReadLine();
        }
    }
}
//todo add fastway to guide user to add stuff
//todo expenses without description
//todo expenses with date
//todo faster way to choose template