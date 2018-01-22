namespace Warlord.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedFilenameToImagepathInVehicle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "Imagepath", c => c.String());
            DropColumn("dbo.Vehicles", "Filename");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Filename", c => c.String());
            DropColumn("dbo.Vehicles", "Imagepath");
        }
    }
}
