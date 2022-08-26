using Microsoft.EntityFrameworkCore.Migrations;

namespace OngProject.Migrations
{
    public partial class pruebaSeedMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Members",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "linkedinUrl",
                table: "Members",
                newName: "LinkedinUrl");

            migrationBuilder.RenameColumn(
                name: "instagramUrl",
                table: "Members",
                newName: "InstagramUrl");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Members",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "facebookUrl",
                table: "Members",
                newName: "FacebookUrl");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Members",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Members",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LinkedinUrl",
                table: "Members",
                newName: "linkedinUrl");

            migrationBuilder.RenameColumn(
                name: "InstagramUrl",
                table: "Members",
                newName: "instagramUrl");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Members",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "FacebookUrl",
                table: "Members",
                newName: "facebookUrl");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Members",
                newName: "description");
        }
    }
}
