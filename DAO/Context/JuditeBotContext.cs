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
        public JuditeBotContext() : base("JuditeBot")
        {

        }
        public DbSet<Users> usuarios { get; set; }
        public DbSet<OrderStatus> statusPedidos { get; set; }
        public DbSet<PaymentMethod> meiosPagamentos { get; set; }
        public DbSet<Order> pedidos { get; set; }
        public DbSet<Product> produtos { get; set; }
        public DbSet<Pizzaria> pizzarias { get; set; }
        public DbSet<ProductInstanceEntity> productInstanceEntity { get; set; }
        //public DbSet<ProductInstance<Beverage>> beverages { get; set; }
        //public DbSet<ProductInstance<Pizza>> pizzas { get; set; }
        public DbSet<Beverage> beverages { get; set; }
        public DbSet<Pizza> pizzas { get; set; }
        public DbSet<MixedPizza> mixedPizzas { get; set; }
        public DbSet<PizzaSize> pizzaSizes { get; set; }
        public DbSet<ProductSize> productSizes { get; set; }
        public DbSet<PedidoTemporario> pedidoTemporario { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var stringConexao = ConfigurationManager.ConnectionStrings[0].ConnectionString;
        //    //string stringConexao = ConfigurationManager.ConnectionStrings["JuditeBotBDConnectionString"].ConnectionString;


        //    optionsBuilder.UseSqlServer(stringConexao);
        //    base.OnConfiguring(optionsBuilder);
        //}

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        //    // Example of controlling TPH iheritance:
        //    //modelBuilder.Entity<Product>()
        //    //        .Map<Beverage>(m => m.Requires("MyType").HasValue("B"))
        //    //        .Map<Pizza>(m => m.Requires("MyType").HasValue("P"));

        //    // Example of controlling Foreign key:
        //    modelBuilder.Entity<Pizzaria>()
        //                .HasMany(p => p.menus)
        //                .WithRequired()
        //                .Map(m => m.MapKey("PizzariaId"));
        //}
    }
}
