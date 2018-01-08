namespace Warlord.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderIdToVehicle : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Vehicles", name: "Order_Id", newName: "OrderId");
            RenameIndex(table: "dbo.Vehicles", name: "IX_Order_Id", newName: "IX_OrderId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Vehicles", name: "IX_OrderId", newName: "IX_Order_Id");
            RenameColumn(table: "dbo.Vehicles", name: "OrderId", newName: "Order_Id");
        }
    }
}
