using Microsoft.EntityFrameworkCore;
using MyMvcApp.Models;

namespace MyMvcApp.Data;

public class ClassroomDbContext : DbContext
{
    public ClassroomDbContext(DbContextOptions<ClassroomDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
}
