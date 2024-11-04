using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swos.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class create_teamkits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_DbSwosKit_AwayKitId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_DbSwosKit_HomeKitId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbSwosKit",
                table: "DbSwosKit");

            migrationBuilder.RenameTable(
                name: "DbSwosKit",
                newName: "TeamKits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamKits",
                table: "TeamKits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_TeamKits_AwayKitId",
                table: "Teams",
                column: "AwayKitId",
                principalTable: "TeamKits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_TeamKits_HomeKitId",
                table: "Teams",
                column: "HomeKitId",
                principalTable: "TeamKits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_TeamKits_AwayKitId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_TeamKits_HomeKitId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamKits",
                table: "TeamKits");

            migrationBuilder.RenameTable(
                name: "TeamKits",
                newName: "DbSwosKit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbSwosKit",
                table: "DbSwosKit",
                column: "Id");

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
        }
    }
}
