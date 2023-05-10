﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SharedServices.Data
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        public string DomainName { get; set; }

        public DateTime DateCreated { get; set; }

        public string Email { get; set; }

        public int Counter { get; set; }

        public bool IsActive { get; set; }

        public string BillingCycle { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BillingAmount { get; set; }

        public DateTime BillingStartDate { get; set; }

        public DateTime BillingEndDate { get; set; }

    }
}

