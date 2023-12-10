using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players { get; set; }
}