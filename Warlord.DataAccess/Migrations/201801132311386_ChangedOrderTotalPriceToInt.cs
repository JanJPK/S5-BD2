namespace Warlord.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedOrderTotalPriceToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "TotalPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "TotalPrice", c => c.Single(nullable: false));
        }
    }
}
