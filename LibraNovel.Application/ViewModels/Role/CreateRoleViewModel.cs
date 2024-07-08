﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Role
{
    public class CreateRoleViewModel
    {
        public string Name { get; set; }
        public int RoleCode { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
