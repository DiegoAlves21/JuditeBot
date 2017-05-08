using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PizzaTamanho
    {
        public int Id { get; set; }
        public string tamanho { get; set; }
        public double fator { get; set; }

        public virtual List<Produto> produtos { get; set; }
    }
}
