using Microsoft.EntityFrameworkCore.Migrations;

namespace OngProject.Migrations
{
    public partial class AddUrlPropertiesToOrganizationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Organization",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "Organization",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedinUrl",
                table: "Organization",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "LinkedinUrl",
                table: "Organization");
        }
    }
}
