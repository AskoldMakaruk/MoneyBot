﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Bot.Types;

namespace MoneyBot.DB.Model
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long ChatId { get; set; }
        public List<ExpenseCategory> Categories { get; set; }
        public List<Person> People { get; set; }
        public List<Fund> Funds { get; set; }
        public AccountStatus Status { get; set; }

        [NotMapped]
        public Expense CurrentExpense { get; set; }

        [NotMapped]
        public Transaction CurrentTransaction { get; set; }

        [NotMapped]
        public Controllers.TelegramController Controller { get; set; }

        [NotMapped]
        public Template CurrentTemplate { get; set; }

        [NotMapped]
        public Message LastMessage { get; set; }

        public static implicit operator ChatId(Account a) => a.ChatId;
    }

    public enum AccountStatus
    {
        Free,
        Start,
        Manage,
        AddCategories,
        EnterExpenseSum,
        EnterTemplate,
        AddPeople,
        EnterTransactionSum,
        ChooseShow,
        OverrideCategories,
        OverridePeople,
    }
}