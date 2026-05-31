using Microsoft.EntityFrameworkCore;
using MyMvcApp.Models;

namespace MyMvcApp.Data;

public class ClassroomDbContext : DbContext
{
    public ClassroomDbContext(DbContextOptions<ClassroomDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Students");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date");

            entity.Property(e => e.EnrolledAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSDATETIME()");
        });
    }
}
