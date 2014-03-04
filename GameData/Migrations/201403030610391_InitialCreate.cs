using System.Data.Entity.Migrations;

namespace GameData.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        GameID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                        Score = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Lines = c.Int(nullable: false),
                        Doubles = c.Int(nullable: false),
                        Tetris = c.Int(nullable: false),
                        BackToBack = c.Int(nullable: false),
                        Tspin = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GameID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 30),
                        Password = c.String(maxLength: 250),
                        Country = c.String(maxLength: 40),
                        RegisteredDate = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "UserID", "dbo.Users");
            DropIndex("dbo.Games", new[] { "UserID" });
            DropTable("dbo.Users");
            DropTable("dbo.Games");
        }
    }
}
