﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Domain.Models;

[Table("Permission")]
public partial class Permission
{
    [Key]
    public int PermissionID { get; set; }

    public int PermissionCode { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string TitleEN { get; set; }

    public int? Parent { get; set; }

    public Guid CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [InverseProperty("ParentNavigation")]
    public virtual ICollection<Permission> InverseParentNavigation { get; set; } = new List<Permission>();

    [ForeignKey("Parent")]
    [InverseProperty("InverseParentNavigation")]
    public virtual Permission ParentNavigation { get; set; }

    [InverseProperty("Permission")]
    public virtual ICollection<RolesPermission> RolesPermissions { get; set; } = new List<RolesPermission>();
}