namespace HistoricalLib.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Models",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Value_TimeStamp = c.DateTime(nullable: false),
                    Value_IdGeographicArea = c.String(),
                    Value_Consumption = c.Int(nullable: false),
                    Code = c.Int(nullable: false),
                    TimeStamp = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Models");
        }
    }
}
