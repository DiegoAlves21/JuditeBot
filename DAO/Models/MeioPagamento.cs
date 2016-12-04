using DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Models
{
    public class MeioPagamento : IEntity
    {
        public int Id { get; set; }
        public enum meioPagamento { DEBITO, CREDITO, SODEXO, TR }
    }
}
