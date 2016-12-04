using Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MeioPagamento : IEntity
    {
        public int Id { get; set; }
        public enum meioPagamento { DEBITO, CREDITO, SODEXO, TR }
    }
}
