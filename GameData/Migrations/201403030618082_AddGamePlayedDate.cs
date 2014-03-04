using System.Data.Entity.Migrations;

namespace GameData.Migrations
{
    public partial class AddGamePlayedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "PlayedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "PlayedDate");
        }
    }
}
