﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Domain.Models;

[Table("Genre")]
public partial class Genre
{
    [Key]
    public int GenreID { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public int? ParentID { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<Genre> InverseParent { get; set; } = new List<Genre>();

    [InverseProperty("Genre")]
    public virtual ICollection<NovelGenre> NovelGenres { get; set; } = new List<NovelGenre>();

    [ForeignKey("ParentID")]
    [InverseProperty("InverseParent")]
    public virtual Genre Parent { get; set; }
}