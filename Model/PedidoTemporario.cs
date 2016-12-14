using Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PedidoTemporario : IEntity
    {
        public int Id { get; set; }
        public long idUsuarioMessenger { get; set; }
        public string nomeUsuarioMessenger { get; set; }
        public int idProduto { get; set; }
        public int idMeioPagamento { get; set; }
        public string endereco { get; set; }
    }
}