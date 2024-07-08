﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraNovel.Domain.Models;

public partial class RolesPermission
{
    [Key]
    public int RolesPermissions { get; set; }

    public int RoleID { get; set; }

    public int PermissionID { get; set; }

    [ForeignKey("PermissionID")]
    [InverseProperty("RolesPermissions")]
    public virtual Permission Permission { get; set; }

    [ForeignKey("RoleID")]
    [InverseProperty("RolesPermissions")]
    public virtual Role Role { get; set; }
}