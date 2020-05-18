namespace HistoricalLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ChangedModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Models", "Value_DateOfCreation", c => c.DateTime(nullable: false));
            DropColumn("dbo.Models", "Value_TimeStamp");
        }

        public override void Down()
        {
            AddColumn("dbo.Models", "Value_TimeStamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.Models", "Value_DateOfCreation");
        }
    }
}
