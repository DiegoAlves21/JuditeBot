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
        public string nome { get; set; }
        public string senha { get; set; }
    }
}
