using System.Data.Entity;
using System.Linq;
using System.Text;
using GameModel.Models;
using GameModel.Models.Database;

namespace GameModel.Contexts
{
    public class StatisticsDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }

        public StatisticsDbContext() : base("TetrisGame")
        {

        }
    }
}
