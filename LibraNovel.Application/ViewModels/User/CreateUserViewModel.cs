﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.User
{
    public class CreateUserViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string CurrentRoom { get; set; }
        public string Device { get; set; }
    }
}
