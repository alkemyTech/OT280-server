using Microsoft.EntityFrameworkCore.Migrations;

namespace OngProject.Migrations
{
    public partial class seedActivities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "welcomeText",
                table: "Organization",
                newName: "WelcomeText");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Organization",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Organization",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Organization",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Organization",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Organization",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "aboutUsText",
                table: "Organization",
                newName: "AboutUsText");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WelcomeText",
                table: "Organization",
                newName: "welcomeText");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Organization",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Organization",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Organization",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Organization",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Organization",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "AboutUsText",
                table: "Organization",
                newName: "aboutUsText");
        }
    }
}
