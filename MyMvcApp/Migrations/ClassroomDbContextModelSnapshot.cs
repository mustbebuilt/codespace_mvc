using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyMvcApp.Data;

#nullable disable

namespace MyMvcApp.Migrations;

[DbContext(typeof(ClassroomDbContext))]
public partial class ClassroomDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.8");

        modelBuilder.Entity("MyMvcApp.Models.Student", b =>
        {
            b.Property<int>("StudentId")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasAnnotation("SqlServer:Identity", "1, 1");

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            b.Property<DateTime>("EnrolledAt")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSDATETIME()");

            b.Property<DateOnly?>("DateOfBirth")
                .HasColumnType("date");

            b.Property<string>("FirstName")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.Property<string>("LastName")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.HasKey("StudentId");

            b.HasIndex("Email")
                .IsUnique();

            b.ToTable("Students");
        });
    }
}