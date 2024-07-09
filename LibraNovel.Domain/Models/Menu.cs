﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Domain.Models;

[Table("Menu")]
public partial class Menu
{
    [Key]
    public int MenuID { get; set; }

    [StringLength(255)]
    public string Title { get; set; }

    [StringLength(100)]
    public string Icon { get; set; }

    [Column(TypeName = "text")]
    public string URL { get; set; }

    [Column(TypeName = "text")]
    public string Path { get; set; }

    public int? ParentID { get; set; }

    public int? OrderBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<Menu> InverseParent { get; set; } = new List<Menu>();

    [ForeignKey("ParentID")]
    [InverseProperty("InverseParent")]
    public virtual Menu Parent { get; set; }

    [InverseProperty("Menu")]
    public virtual ICollection<RolesMenu> RolesMenus { get; set; } = new List<RolesMenu>();
}