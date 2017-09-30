using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuditeBot.Bot
{
    [Serializable]
    public class BebidasSelecionadas
    {
        //public Dictionary<int, string> bebidas { get; set; }
        public int key { get; set; }
        public string value { get; set; }
        public string tamanho { get; set; }
    }
}