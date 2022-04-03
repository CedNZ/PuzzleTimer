using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzzleTimer.Migrations
{
    /// <inheritdoc />
    public partial class Users_to_SolvingSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolvingSessionUser",
                columns: table => new
                {
                    SolvingSessionsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolvingSessionUser", x => new { x.SolvingSessionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SolvingSessionUser_SolvingSessions_SolvingSessionsId",
                        column: x => x.SolvingSessionsId,
                        principalTable: "SolvingSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolvingSessionUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolvingSessionUser_UsersId",
                table: "SolvingSessionUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolvingSessionUser");
        }
    }
}
