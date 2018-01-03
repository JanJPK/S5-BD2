namespace Warlord.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRelations : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Orders", name: "Customer_Id", newName: "CustomerId");
            RenameColumn(table: "dbo.Vehicles", name: "VehicleModel_Id", newName: "VehicleModelId");
            RenameColumn(table: "dbo.VehicleModels", name: "Manufacturer_Id", newName: "ManufacturerId");
            RenameIndex(table: "dbo.Orders", name: "IX_Customer_Id", newName: "IX_CustomerId");
            RenameIndex(table: "dbo.Vehicles", name: "IX_VehicleModel_Id", newName: "IX_VehicleModelId");
            RenameIndex(table: "dbo.VehicleModels", name: "IX_Manufacturer_Id", newName: "IX_ManufacturerId");
            AddColumn("dbo.Manufacturers", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Manufacturers", "RowVersion");
            RenameIndex(table: "dbo.VehicleModels", name: "IX_ManufacturerId", newName: "IX_Manufacturer_Id");
            RenameIndex(table: "dbo.Vehicles", name: "IX_VehicleModelId", newName: "IX_VehicleModel_Id");
            RenameIndex(table: "dbo.Orders", name: "IX_CustomerId", newName: "IX_Customer_Id");
            RenameColumn(table: "dbo.VehicleModels", name: "ManufacturerId", newName: "Manufacturer_Id");
            RenameColumn(table: "dbo.Vehicles", name: "VehicleModelId", newName: "VehicleModel_Id");
            RenameColumn(table: "dbo.Orders", name: "CustomerId", newName: "Customer_Id");
        }
    }
}
