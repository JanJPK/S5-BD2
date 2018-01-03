namespace Warlord.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Phone = c.Int(),
                        PostalCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Manufacturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Country = c.String(nullable: false, maxLength: 20),
                        FullName = c.String(nullable: false, maxLength: 50),
                        ShortName = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Completed = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        TotalPrice = c.Single(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Color = c.String(nullable: false, maxLength: 30),
                        Condition = c.String(),
                        DateOfManufacture = c.DateTime(nullable: false),
                        Filename = c.String(),
                        Price = c.Single(nullable: false),
                        VehicleModel_Id = c.Int(nullable: false),
                        Order_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleModels", t => t.VehicleModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.VehicleModel_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.VehicleModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        MainArmament = c.String(nullable: false, maxLength: 30),
                        SecondaryArmament = c.String(maxLength: 30),
                        Crew = c.Int(nullable: false),
                        Weight = c.Single(nullable: false),
                        Engine = c.String(nullable: false, maxLength: 20),
                        EnginePower = c.Int(nullable: false),
                        Manufacturer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Manufacturers", t => t.Manufacturer_Id, cascadeDelete: true)
                .Index(t => t.Manufacturer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Vehicles", "VehicleModel_Id", "dbo.VehicleModels");
            DropForeignKey("dbo.VehicleModels", "Manufacturer_Id", "dbo.Manufacturers");
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.VehicleModels", new[] { "Manufacturer_Id" });
            DropIndex("dbo.Vehicles", new[] { "Order_Id" });
            DropIndex("dbo.Vehicles", new[] { "VehicleModel_Id" });
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropTable("dbo.VehicleModels");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Orders");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Customers");
        }
    }
}
