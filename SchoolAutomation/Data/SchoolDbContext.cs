using Microsoft.EntityFrameworkCore;
using SchoolAutomation.Models.DbModels;

namespace SchoolAutomation.Data;

public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Science> Sciences { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>()
            .HasIndex(a => a.Username)
            .IsUnique();
        modelBuilder.Entity<Teacher>()
            .HasIndex(t => t.Username)
            .IsUnique();
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Username)
            .IsUnique();


        base.OnModelCreating(modelBuilder);
    }
}
