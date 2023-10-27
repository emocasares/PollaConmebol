using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollaEngendrilClientHosted.Server.Migrations
{
    public partial class AddNickNameSupportToUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "NickName");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "Users",
                newName: "UserName");
        }
    }
}
