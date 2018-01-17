using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.PosMachine
{
    public class StatusModel
    {
        public Enumeration.MerchantPosMachineStatus Status { get; set; }

        public decimal Deposit { get; set; }

        public decimal Rent { get; set; }

        public DateTime RentDueDate { get; set; }

    }
}