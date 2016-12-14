using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Model;
using System.Data.Entity;

namespace DAO.Context
{
    public class JuditeBotContext : DbContext
    {
        public JuditeBotContext() : base("JuditeBot")
        {

        }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<StatusPedido> statusPedidos { get; set; }
        public DbSet<MeioPagamento> meiosPagamentos { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        public DbSet<Produto> produtos { get; set; }
        public DbSet<Pizzaria> pizzarias { get; set; }
        public DbSet<PedidoTemporario> pedidoTemporario { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var stringConexao = ConfigurationManager.ConnectionStrings[0].ConnectionString;
        //    //string stringConexao = ConfigurationManager.ConnectionStrings["JuditeBotBDConnectionString"].ConnectionString;


        //    optionsBuilder.UseSqlServer(stringConexao);
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
