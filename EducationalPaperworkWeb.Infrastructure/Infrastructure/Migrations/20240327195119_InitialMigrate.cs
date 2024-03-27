using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalPaperworkWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "Chats",
    columns: table => new
    {
        Id = table.Column<long>(type: "bigint", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
        StudentId = table.Column<long>(type: "bigint", nullable: false),
        AdminId = table.Column<long>(type: "bigint", nullable: false),
        TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
        IsTaken = table.Column<bool>(type: "bit", nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Chats", x => x.Id);
    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
