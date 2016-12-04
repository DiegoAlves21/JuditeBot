﻿using DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Models
{
    public class StatusPedido : IEntity
    {
        public int Id { get; set; }
        public enum statusPedido { EM_ESPERA, SAIU_PARA_ENTREGA, ENTREGUE }
    }
}
