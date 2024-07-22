﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Domain.Models;

[Table("Transaction")]
public partial class Transaction
{
    [Key]
    [StringLength(20)]
    public string TransactionID { get; set; }

    public int CardID { get; set; }

    [Required]
    [StringLength(255)]
    public string Amount { get; set; }

    [StringLength(255)]
    public string TransactionDate { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    [StringLength(10)]
    public string BankCode { get; set; }

    [StringLength(20)]
    public string BankTranNo { get; set; }

    [StringLength(10)]
    public string CardType { get; set; }

    [StringLength(2)]
    public string ResponseCode { get; set; }

    [StringLength(20)]
    public string TnxRef { get; set; }

    [StringLength(10)]
    public string TransactionStatus { get; set; }

    [ForeignKey("CardID")]
    [InverseProperty("Transactions")]
    public virtual Card Card { get; set; }
}