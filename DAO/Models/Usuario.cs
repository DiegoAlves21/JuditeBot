using DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Models
{
    public class Usuario : IEntity
    {
        public int Id { get; set; }
        public string nome { get; set; }
        public string usuario { get; set; }
    }
}
