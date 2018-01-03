namespace Warlord.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRowVersions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Orders", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Vehicles", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.VehicleModels", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterColumn("dbo.Customers", "Address", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "City", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "Country", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customers", "PostalCode", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.VehicleModels", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.VehicleModels", "MainArmament", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.VehicleModels", "SecondaryArmament", c => c.String(maxLength: 50));
            AlterColumn("dbo.VehicleModels", "Engine", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Manufacturers", "Country", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Manufacturers", "ShortName", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Manufacturers", "ShortName", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Manufacturers", "Country", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.VehicleModels", "Engine", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.VehicleModels", "SecondaryArmament", c => c.String(maxLength: 30));
            AlterColumn("dbo.VehicleModels", "MainArmament", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.VehicleModels", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Customers", "PostalCode", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Country", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Address", c => c.String(nullable: false));
            DropColumn("dbo.VehicleModels", "RowVersion");
            DropColumn("dbo.Vehicles", "RowVersion");
            DropColumn("dbo.Orders", "RowVersion");
            DropColumn("dbo.Customers", "RowVersion");
        }
    }
}
