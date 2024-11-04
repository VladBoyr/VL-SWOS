using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swos.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class create_globalteams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalTeams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GlobalTeamSwos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GlobalTeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    SwosTeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalTeamSwos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlobalTeamSwos_GlobalTeams_GlobalTeamId",
                        column: x => x.GlobalTeamId,
                        principalTable: "GlobalTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GlobalTeamSwos_Teams_SwosTeamId",
                        column: x => x.SwosTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalTeamSwos_GlobalTeamId",
                table: "GlobalTeamSwos",
                column: "GlobalTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalTeamSwos_SwosTeamId",
                table: "GlobalTeamSwos",
                column: "SwosTeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalTeamSwos");

            migrationBuilder.DropTable(
                name: "GlobalTeams");
        }
    }
}
