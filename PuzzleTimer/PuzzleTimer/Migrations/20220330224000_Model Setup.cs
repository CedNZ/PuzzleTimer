using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PuzzleTimer.Migrations
{
    /// <inheritdoc />
    public partial class ModelSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Puzzles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PieceCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puzzles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolvingSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PuzzleId = table.Column<int>(type: "int", nullable: true),
                    Started = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Completed = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolvingSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolvingSessions_Puzzles_PuzzleId",
                        column: x => x.PuzzleId,
                        principalTable: "Puzzles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TimeEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SolvingSessionId = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeEntries_SolvingSessions_SolvingSessionId",
                        column: x => x.SolvingSessionId,
                        principalTable: "SolvingSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolvingSessions_PuzzleId",
                table: "SolvingSessions",
                column: "PuzzleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_SolvingSessionId",
                table: "TimeEntries",
                column: "SolvingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_UserId",
                table: "TimeEntries",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeEntries");

            migrationBuilder.DropTable(
                name: "SolvingSessions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Puzzles");
        }
    }
}
