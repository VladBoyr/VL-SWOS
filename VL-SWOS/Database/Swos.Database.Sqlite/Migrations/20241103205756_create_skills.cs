using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swos.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class create_skills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbSwosSkill_Players_PlayerId",
                table: "DbSwosSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbSwosSkill",
                table: "DbSwosSkill");

            migrationBuilder.RenameTable(
                name: "DbSwosSkill",
                newName: "Skills");

            migrationBuilder.RenameIndex(
                name: "IX_DbSwosSkill_PlayerId",
                table: "Skills",
                newName: "IX_Skills_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Players_PlayerId",
                table: "Skills",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Players_PlayerId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "DbSwosSkill");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_PlayerId",
                table: "DbSwosSkill",
                newName: "IX_DbSwosSkill_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbSwosSkill",
                table: "DbSwosSkill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbSwosSkill_Players_PlayerId",
                table: "DbSwosSkill",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
