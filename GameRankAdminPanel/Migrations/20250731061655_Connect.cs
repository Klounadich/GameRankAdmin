using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRankAdminPanel.Migrations
{
    /// <inheritdoc />
    public partial class Connect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDataAdmin",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IPAdress = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDataAdmin", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDataAdmin");
        }
    }
}
