using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<TurbineAction> TurbineActions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Measurement>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.turbineId).IsRequired();
        });

        modelBuilder.Entity<Alert>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.turbineId).IsRequired();
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.UserId);
        });

        modelBuilder.Entity<TurbineAction>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId);
        });
    }
}