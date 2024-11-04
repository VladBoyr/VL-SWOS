using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swos.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class init_teamdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbSwosKit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KitType = table.Column<byte>(type: "INTEGER", nullable: false),
                    ShirtMainColor = table.Column<byte>(type: "INTEGER", nullable: false),
                    ShirtExtraColor = table.Column<byte>(type: "INTEGER", nullable: false),
                    ShortsColor = table.Column<byte>(type: "INTEGER", nullable: false),
                    SocksColor = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSwosKit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbSwosPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Number = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<byte>(type: "INTEGER", nullable: false),
                    Face = table.Column<byte>(type: "INTEGER", nullable: false),
                    Position = table.Column<byte>(type: "INTEGER", nullable: false),
                    Rating = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSwosPlayer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamDatabases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamDatabases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbSwosSkill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Skill = table.Column<byte>(type: "INTEGER", nullable: false),
                    SkillValue = table.Column<byte>(type: "INTEGER", nullable: false),
                    PrimarySkill = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSwosSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbSwosSkill_DbSwosPlayer_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "DbSwosPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbSwosTeam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamDatabaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    GlobalId = table.Column<int>(type: "INTEGER", nullable: false),
                    LocalId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<byte>(type: "INTEGER", nullable: false),
                    Division = table.Column<byte>(type: "INTEGER", nullable: false),
                    HomeKitId = table.Column<int>(type: "INTEGER", nullable: false),
                    AwayKitId = table.Column<int>(type: "INTEGER", nullable: false),
                    CoachName = table.Column<string>(type: "TEXT", nullable: false),
                    Tactic = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSwosTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbSwosTeam_DbSwosKit_AwayKitId",
                        column: x => x.AwayKitId,
                        principalTable: "DbSwosKit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbSwosTeam_DbSwosKit_HomeKitId",
                        column: x => x.HomeKitId,
                        principalTable: "DbSwosKit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbSwosTeam_TeamDatabases_TeamDatabaseId",
                        column: x => x.TeamDatabaseId,
                        principalTable: "TeamDatabases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbSwosTeamPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerPositionIndex = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSwosTeamPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbSwosTeamPlayer_DbSwosPlayer_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "DbSwosPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbSwosTeamPlayer_DbSwosTeam_TeamId",
                        column: x => x.TeamId,
                        principalTable: "DbSwosTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbSwosSkill_PlayerId",
                table: "DbSwosSkill",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_DbSwosTeam_AwayKitId",
                table: "DbSwosTeam",
                column: "AwayKitId");

            migrationBuilder.CreateIndex(
                name: "IX_DbSwosTeam_HomeKitId",
                table: "DbSwosTeam",
                column: "HomeKitId");

            migrationBuilder.CreateIndex(
                name: "IX_DbSwosTeam_TeamDatabaseId",
                table: "DbSwosTeam",
                column: "TeamDatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DbSwosTeamPlayer_PlayerId",
                table: "DbSwosTeamPlayer",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_DbSwosTeamPlayer_TeamId",
                table: "DbSwosTeamPlayer",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbSwosSkill");

            migrationBuilder.DropTable(
                name: "DbSwosTeamPlayer");

            migrationBuilder.DropTable(
                name: "DbSwosPlayer");

            migrationBuilder.DropTable(
                name: "DbSwosTeam");

            migrationBuilder.DropTable(
                name: "DbSwosKit");

            migrationBuilder.DropTable(
                name: "TeamDatabases");
        }
    }
}
