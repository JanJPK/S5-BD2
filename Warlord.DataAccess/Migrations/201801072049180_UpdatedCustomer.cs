namespace Warlord.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.Customers", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "Phone", c => c.Int());
            DropColumn("dbo.Customers", "Email");
        }
    }
}
