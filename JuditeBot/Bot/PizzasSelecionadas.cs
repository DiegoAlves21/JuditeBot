﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuditeBot.Bot
{
    [Serializable]
    public class PizzasSelecionadas
    {
        public Dictionary<int,string> sabores { get; set; }
        public string tamanho { get; set; }
    }
}