namespace Warlord.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleIncreasedColorStringLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vehicles", "Color", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vehicles", "Color", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
