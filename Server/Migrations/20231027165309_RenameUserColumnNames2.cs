using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollaEngendrilClientHosted.Server.Migrations
{
    public partial class RenameUserColumnNames2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Users] SET [Name] = [Nickname], [Nickname] = [Name]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Users] SET [Nickname] = [Name], [Name] = [Nickname]");
        }
    }
}
