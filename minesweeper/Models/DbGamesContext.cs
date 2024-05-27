
using Microsoft.EntityFrameworkCore;


namespace minesweeper.Models
{
    public class DbGamesContext : DbContext
    {
        public DbSet<Game> games { get; set; }

        public DbGamesContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-GTMM6OH\\SQLEXPRESS;Database=Minesweeper;Trusted_Connection=true;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
