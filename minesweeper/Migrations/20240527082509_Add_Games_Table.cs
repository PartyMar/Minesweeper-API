using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace minesweeper.Migrations
{
    /// <inheritdoc />
    public partial class Add_Games_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    game_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    width = table.Column<int>(type: "int", nullable: false),
                    height = table.Column<int>(type: "int", nullable: false),
                    mines_count = table.Column<int>(type: "int", nullable: false),
                    mineField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    playerField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games");
        }
    }
}
