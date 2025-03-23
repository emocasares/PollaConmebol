using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollaEngendrilClientHosted.Server.Migrations
{
    public partial class AddPreloadPointsPerUserTableAndSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreloadPointsPerUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InitialPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreloadPointsPerUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreloadPointsPerUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Insert initial data
            migrationBuilder.InsertData(
                table: "PreloadPointsPerUsers",
                columns: new[] { "UserId", "InitialPoints" },
                values: new object[,]
                {
                        { 19, 113 },
                        { 17, 101 },
                        { 9, 100 },
                        { 13, 93 },
                        { 16, 88 },
                        { 12, 71 },
                        { 18, 71 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreloadPointsPerUsers_UserId",
                table: "PreloadPointsPerUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreloadPointsPerUsers");
        }
    }
}
