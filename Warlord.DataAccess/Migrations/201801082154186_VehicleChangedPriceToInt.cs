namespace Warlord.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleChangedPriceToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vehicles", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vehicles", "Price", c => c.Single(nullable: false));
        }
    }
}
