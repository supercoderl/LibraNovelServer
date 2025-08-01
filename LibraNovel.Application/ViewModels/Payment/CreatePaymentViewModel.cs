﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Payment
{
    public class CreatePayOSViewModel
    {
        public string ProductName {  get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }

    public class OrderInfo
    {
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public string OrderDesc { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public long PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }
    }

    public class SessionStripe
    {
        public long Amount { get; set; }
        public string Currency {  get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string Mode { get; set; }
        public string? CustomerEmail { get; set; }
    }
}
