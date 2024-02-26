using Microsoft.EntityFrameworkCore;

namespace SuperHeroAPI;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<SuperHero> SuperHeroes { get; set; }
}
