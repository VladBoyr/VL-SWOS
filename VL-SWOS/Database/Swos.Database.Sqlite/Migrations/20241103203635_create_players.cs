using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swos.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class create_players : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosSkill_DbSwosPlayer_PlayerId",
                table: "DbSwosSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosTeamPlayer_DbSwosPlayer_PlayerId",
                table: "DbSwosTeamPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosTeamPlayer_Teams_TeamId",
                table: "DbSwosTeamPlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbSwosTeamPlayer",
                table: "DbSwosTeamPlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbSwosPlayer",
                table: "DbSwosPlayer");

            migrationBuilder.RenameTable(
                name: "DbSwosTeamPlayer",
                newName: "TeamPlayers");

            migrationBuilder.RenameTable(
                name: "DbSwosPlayer",
                newName: "Players");

            migrationBuilder.RenameIndex(
                name: "IX_DbSwosTeamPlayer_TeamId",
                table: "TeamPlayers",
                newName: "IX_TeamPlayers_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_DbSwosTeamPlayer_PlayerId",
                table: "TeamPlayers",
                newName: "IX_TeamPlayers_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamPlayers",
                table: "TeamPlayers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosSkill_Players_PlayerId",
                table: "DbSwosSkill",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayers_Players_PlayerId",
                table: "TeamPlayers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayers_Teams_TeamId",
                table: "TeamPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosSkill_Players_PlayerId",
                table: "DbSwosSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayers_Players_PlayerId",
                table: "TeamPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayers_Teams_TeamId",
                table: "TeamPlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamPlayers",
                table: "TeamPlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.RenameTable(
                name: "TeamPlayers",
                newName: "DbSwosTeamPlayer");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "DbSwosPlayer");

            migrationBuilder.RenameIndex(
                name: "IX_TeamPlayers_TeamId",
                table: "DbSwosTeamPlayer",
                newName: "IX_DbSwosTeamPlayer_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamPlayers_PlayerId",
                table: "DbSwosTeamPlayer",
                newName: "IX_DbSwosTeamPlayer_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbSwosTeamPlayer",
                table: "DbSwosTeamPlayer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbSwosPlayer",
                table: "DbSwosPlayer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosSkill_DbSwosPlayer_PlayerId",
                table: "DbSwosSkill",
                column: "PlayerId",
                principalTable: "DbSwosPlayer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosTeamPlayer_DbSwosPlayer_PlayerId",
                table: "DbSwosTeamPlayer",
                column: "PlayerId",
                principalTable: "DbSwosPlayer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosTeamPlayer_Teams_TeamId",
                table: "DbSwosTeamPlayer",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
