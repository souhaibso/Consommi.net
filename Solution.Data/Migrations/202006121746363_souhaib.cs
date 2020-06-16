namespace Solution.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class souhaib : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartLines",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        dateAjout = c.DateTime(nullable: false),
                        quantiteChoisie = c.Int(nullable: false),
                        prixDuProduit = c.Double(nullable: false),
                        prixTotal = c.Double(nullable: false),
                        productId = c.Int(nullable: false),
                        CartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Carts", t => t.CartId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.productId, cascadeDelete: true)
                .Index(t => t.productId)
                .Index(t => t.CartId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        prixTotal = c.Double(nullable: false),
                        status = c.Boolean(nullable: false),
                        dateAchat = c.DateTime(nullable: false),
                        userId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nom = c.String(),
                        prix = c.Int(nullable: false),
                        quantite = c.Int(nullable: false),
                        description = c.String(),
                        imageString = c.String(),
                        imageByte = c.Binary(),
                        Categorie = c.String(),
                        dateAjout = c.DateTime(nullable: false),
                        userId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        CIN = c.String(nullable: false, maxLength: 128),
                        DateNaissance = c.DateTime(nullable: false),
                        Mail = c.String(),
                        Nom = c.String(),
                        Prenom = c.String(),
                    })
                .PrimaryKey(t => t.CIN);
            
            CreateTable(
                "dbo.Factures",
                c => new
                    {
                        IdF = c.Int(nullable: false, identity: true),
                        ClientFK = c.String(),
                        ProductFK = c.Int(nullable: false),
                        DateAchat = c.DateTime(nullable: false),
                        Prix = c.Double(nullable: false),
                        MyClient_CIN = c.String(maxLength: 128),
                        MyProduct_id = c.Int(),
                    })
                .PrimaryKey(t => t.IdF)
                .ForeignKey("dbo.Clients", t => t.MyClient_CIN)
                .ForeignKey("dbo.Products", t => t.MyProduct_id)
                .Index(t => t.MyClient_CIN)
                .Index(t => t.MyProduct_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Factures", "MyProduct_id", "dbo.Products");
            DropForeignKey("dbo.Factures", "MyClient_CIN", "dbo.Clients");
            DropForeignKey("dbo.CartLines", "productId", "dbo.Products");
            DropForeignKey("dbo.CartLines", "CartId", "dbo.Carts");
            DropIndex("dbo.Factures", new[] { "MyProduct_id" });
            DropIndex("dbo.Factures", new[] { "MyClient_CIN" });
            DropIndex("dbo.CartLines", new[] { "CartId" });
            DropIndex("dbo.CartLines", new[] { "productId" });
            DropTable("dbo.Factures");
            DropTable("dbo.Clients");
            DropTable("dbo.Products");
            DropTable("dbo.Carts");
            DropTable("dbo.CartLines");
        }
    }
}
