using System.Data.Entity.Migrations;
using System.Linq;
using GameData.Contexts;

namespace GameData.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<StatisticsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GameModel.Contexts.StatisticsDbContext";
        }

        protected override void Seed(StatisticsDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
