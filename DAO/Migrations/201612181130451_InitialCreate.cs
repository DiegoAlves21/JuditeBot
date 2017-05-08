namespace DAO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeioPagamentoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        meioPagamento = c.String(),
                        Pizzaria_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pizzarias", t => t.Pizzaria_Id)
                .Index(t => t.Pizzaria_Id);
            
            CreateTable(
                "dbo.Pedidoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        nomeCliente = c.String(),
                        endereco = c.String(),
                        criadoQuando = c.DateTime(nullable: false),
                        statusId = c.Int(nullable: false),
                        meioPagamentoId = c.Int(nullable: false),
                        Pizzaria_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MeioPagamentoes", t => t.meioPagamentoId, cascadeDelete: true)
                .ForeignKey("dbo.StatusPedidoes", t => t.statusId, cascadeDelete: true)
                .ForeignKey("dbo.Pizzarias", t => t.Pizzaria_Id)
                .Index(t => t.statusId)
                .Index(t => t.meioPagamentoId)
                .Index(t => t.Pizzaria_Id);
            
            CreateTable(
                "dbo.Produtoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        nome = c.String(),
                        valor = c.Double(nullable: false),
                        Pedido_Id = c.Int(),
                        Pizzaria_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pedidoes", t => t.Pedido_Id)
                .ForeignKey("dbo.Pizzarias", t => t.Pizzaria_Id)
                .Index(t => t.Pedido_Id)
                .Index(t => t.Pizzaria_Id);
            
            CreateTable(
                "dbo.StatusPedidoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        statusPedido = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PedidoTemporarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        idUsuarioMessenger = c.Long(nullable: false),
                        nomeUsuarioMessenger = c.String(),
                        idProduto = c.Int(nullable: false),
                        idMeioPagamento = c.Int(nullable: false),
                        endereco = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pizzarias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        nome = c.String(),
                        taxaEntrega = c.Double(nullable: false),
                        usuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.usuarioId, cascadeDelete: true)
                .Index(t => t.usuarioId);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pizzarias", "usuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.Pedidoes", "Pizzaria_Id", "dbo.Pizzarias");
            DropForeignKey("dbo.MeioPagamentoes", "Pizzaria_Id", "dbo.Pizzarias");
            DropForeignKey("dbo.Produtoes", "Pizzaria_Id", "dbo.Pizzarias");
            DropForeignKey("dbo.Pedidoes", "statusId", "dbo.StatusPedidoes");
            DropForeignKey("dbo.Produtoes", "Pedido_Id", "dbo.Pedidoes");
            DropForeignKey("dbo.Pedidoes", "meioPagamentoId", "dbo.MeioPagamentoes");
            DropIndex("dbo.Pizzarias", new[] { "usuarioId" });
            DropIndex("dbo.Produtoes", new[] { "Pizzaria_Id" });
            DropIndex("dbo.Produtoes", new[] { "Pedido_Id" });
            DropIndex("dbo.Pedidoes", new[] { "Pizzaria_Id" });
            DropIndex("dbo.Pedidoes", new[] { "meioPagamentoId" });
            DropIndex("dbo.Pedidoes", new[] { "statusId" });
            DropIndex("dbo.MeioPagamentoes", new[] { "Pizzaria_Id" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.Pizzarias");
            DropTable("dbo.PedidoTemporarios");
            DropTable("dbo.StatusPedidoes");
            DropTable("dbo.Produtoes");
            DropTable("dbo.Pedidoes");
            DropTable("dbo.MeioPagamentoes");
        }
    }
}
