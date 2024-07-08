﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Domain.Models;

[Table("Rating")]
public partial class Rating
{
    [Key]
    public int RatingID { get; set; }

    public int Score { get; set; }

    public Guid? UserID { get; set; }

    public int? NovelID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("NovelID")]
    [InverseProperty("Ratings")]
    public virtual Novel Novel { get; set; }

    [ForeignKey("UserID")]
    [InverseProperty("Ratings")]
    public virtual User User { get; set; }
}