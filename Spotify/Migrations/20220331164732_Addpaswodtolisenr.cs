using Microsoft.EntityFrameworkCore.Migrations;

namespace Spotify.Migrations
{
    public partial class Addpaswodtolisenr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Listeners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Listeners");
        }
    }
}
