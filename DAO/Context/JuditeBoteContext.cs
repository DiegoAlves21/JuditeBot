using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.Models;
using Microsoft.Data.Entity;
using System.Configuration;

namespace DAO.Context
{
    public class JuditeBoteContext : DbContext
    {
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<StatusPedido> statusPedidos { get; set; }
        public DbSet<MeioPagamento> meiosPagamentos { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        public DbSet<Produto> produtos { get; set; }
        public DbSet<Pizzaria> pizzarias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string stringConexao = ConfigurationManager.ConnectionStrings["JuditeBotBDConnectionString"].ConnectionString;
            optionsBuilder.UseSqlServer(stringConexao);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
