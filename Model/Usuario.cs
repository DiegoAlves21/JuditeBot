﻿using Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Usuario : IEntity
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
