using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MyMvcApp.Data;

#nullable disable

namespace MyMvcApp.Migrations;

[DbContext(typeof(ClassroomDbContext))]
[Migration("20260531120000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Students",
            columns: table => new
            {
                StudentId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                EnrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Students", x => x.StudentId);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Students_Email",
            table: "Students",
            column: "Email",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Students");
    }
}