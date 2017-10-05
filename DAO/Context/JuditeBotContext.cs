using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Model;
using System.Data.Entity;
using Model.Procucts;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAO.Context
{
    public class JuditeBotContext : DbContext
    {
        public JuditeBotContext() : base("name=juditebotdbEntities")
        {

        }
        public DbSet<Users> usuarios { get; set; }
        public DbSet<Order> pedidos { get; set; }
        public DbSet<Product> produtos { get; set; }
        public DbSet<Pizzaria> pizzarias { get; set; }
        public DbSet<ProductInstance> productInstance { get; set; }
        public DbSet<MixedPizza> mixedPizzas { get; set; }
        public DbSet<ProductSize> productSizes { get; set; }
        public DbSet<PedidoTemporario> pedidoTemporario { get; set; }
        public DbSet<CPaymentMethod> paymentMethod { get; set; }
        public DbSet<Client> client { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
