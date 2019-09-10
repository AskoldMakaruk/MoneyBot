﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyBot.DB.Model
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long ChatId { get; set; }
        public List<ExspenseCategory> Categories { get; set; }
        public List<Exspense> Exspenses { get; set; }
        public AccountStatus Status { get; set; }

        [NotMapped]
        public Exspense CurrentExspense { get; set; }
    }

    public enum AccountStatus
    {
        Free,
        Start,
        Manage,
        AddCategory,
        EditCategory,
        EnterExspenseSum,
    }
}