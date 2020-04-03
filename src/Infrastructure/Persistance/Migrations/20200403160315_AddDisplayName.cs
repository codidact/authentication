using Microsoft.EntityFrameworkCore.Migrations;

namespace Codidact.Authentication.Infrastructure.Persistance.Migrations
{
    public partial class AddDisplayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "identity_user",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "identity_user");
        }
    }
}
