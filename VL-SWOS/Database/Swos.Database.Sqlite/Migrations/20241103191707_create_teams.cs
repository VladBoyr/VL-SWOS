using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swos.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class create_teams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosTeam_DbSwosKit_AwayKitId",
                table: "DbSwosTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosTeam_DbSwosKit_HomeKitId",
                table: "DbSwosTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosTeam_TeamDatabases_TeamDatabaseId",
                table: "DbSwosTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosTeamPlayer_DbSwosTeam_TeamId",
                table: "DbSwosTeamPlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbSwosTeam",
                table: "DbSwosTeam");

            migrationBuilder.RenameTable(
                name: "DbSwosTeam",
                newName: "Teams");

            migrationBuilder.RenameIndex(
                name: "IX_DbSwosTeam_TeamDatabaseId",
                table: "Teams",
                newName: "IX_Teams_TeamDatabaseId");

            migrationBuilder.RenameIndex(
                name: "IX_DbSwosTeam_HomeKitId",
                table: "Teams",
                newName: "IX_Teams_HomeKitId");

            migrationBuilder.RenameIndex(
                name: "IX_DbSwosTeam_AwayKitId",
                table: "Teams",
                newName: "IX_Teams_AwayKitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosTeamPlayer_Teams_TeamId",
                table: "DbSwosTeamPlayer",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_DbSwosKit_AwayKitId",
                table: "Teams",
                column: "AwayKitId",
                principalTable: "DbSwosKit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_DbSwosKit_HomeKitId",
                table: "Teams",
                column: "HomeKitId",
                principalTable: "DbSwosKit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_TeamDatabases_TeamDatabaseId",
                table: "Teams",
                column: "TeamDatabaseId",
                principalTable: "TeamDatabases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosTeamPlayer_Teams_TeamId",
                table: "DbSwosTeamPlayer");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_DbSwosKit_AwayKitId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_DbSwosKit_HomeKitId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_TeamDatabases_TeamDatabaseId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "DbSwosTeam");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_TeamDatabaseId",
                table: "DbSwosTeam",
                newName: "IX_DbSwosTeam_TeamDatabaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_HomeKitId",
                table: "DbSwosTeam",
                newName: "IX_DbSwosTeam_HomeKitId");

            migrationBuilder.RenameIndex(
                name: "IX_Teams_AwayKitId",
                table: "DbSwosTeam",
                newName: "IX_DbSwosTeam_AwayKitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbSwosTeam",
                table: "DbSwosTeam",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosTeam_DbSwosKit_AwayKitId",
                table: "DbSwosTeam",
                column: "AwayKitId",
                principalTable: "DbSwosKit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosTeam_DbSwosKit_HomeKitId",
                table: "DbSwosTeam",
                column: "HomeKitId",
                principalTable: "DbSwosKit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosTeam_TeamDatabases_TeamDatabaseId",
                table: "DbSwosTeam",
                column: "TeamDatabaseId",
                principalTable: "TeamDatabases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosTeamPlayer_DbSwosTeam_TeamId",
                table: "DbSwosTeamPlayer",
                column: "TeamId",
                principalTable: "DbSwosTeam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
