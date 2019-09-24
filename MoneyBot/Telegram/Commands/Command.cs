﻿using MoneyBot.DB.Model;
using Telegram.Bot.Types;

namespace MoneyBot.Telegram.Commands
{
    public abstract class Command
    {
        // 0 not at all
        // 1 just handle reply
        // 2 main condition is true
        // 3 role staff
        // 4 high priority
        public abstract int Suitability(Message message, Account account);
        public abstract OutMessage Execute(Message message, Account account);
        public virtual bool Canceled(Message message, Account account)
        {
            return message.Text.ToLower().Equals("cancel") ||
                message.Text.ToLower().Equals("/cancel");
        }
        public virtual OutMessage Relieve(Message message, Account account)
        {
            return new MainCommand().Execute(message, account);
        }
    }
}