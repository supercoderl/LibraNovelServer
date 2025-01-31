﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Domain.Models;

[Table("Token")]
public partial class Token
{
    [Key]
    public int TokenID { get; set; }

    public string RefreshToken { get; set; }

    public Guid? UserID { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RevokeAt { get; set; }

    [ForeignKey("UserID")]
    [InverseProperty("Tokens")]
    public virtual User User { get; set; }
}