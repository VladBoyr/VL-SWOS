using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swos.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class create_GlobalPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalPlayers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GlobalPlayerSwos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GlobalPlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SwosPlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalPlayerSwos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlobalPlayerSwos_GlobalPlayers_GlobalPlayerId",
                        column: x => x.GlobalPlayerId,
                        principalTable: "GlobalPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GlobalPlayerSwos_Players_SwosPlayerId",
                        column: x => x.SwosPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalPlayerSwos_GlobalPlayerId",
                table: "GlobalPlayerSwos",
                column: "GlobalPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalPlayerSwos_SwosPlayerId",
                table: "GlobalPlayerSwos",
                column: "SwosPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalPlayerSwos");

            migrationBuilder.DropTable(
                name: "GlobalPlayers");
        }
    }
}
