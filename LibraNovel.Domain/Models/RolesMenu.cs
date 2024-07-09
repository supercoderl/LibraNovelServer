﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Domain.Models;

public partial class RolesMenu
{
    [Key]
    public int RolesMenusID { get; set; }

    public int RoleID { get; set; }

    public int MenuID { get; set; }

    [ForeignKey("MenuID")]
    [InverseProperty("RolesMenus")]
    public virtual Menu Menu { get; set; }

    [ForeignKey("RoleID")]
    [InverseProperty("RolesMenus")]
    public virtual Role Role { get; set; }
}