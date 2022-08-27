using Microsoft.EntityFrameworkCore.Migrations;

namespace OngProject.Migrations
{
    public partial class Categoriesseeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Categories_categoryId",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "News",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "News",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "News",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "News",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_News_categoryId",
                table: "News",
                newName: "IX_News_CategoryId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Categories",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Categories",
                newName: "Description");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Categories_CategoryId",
                table: "News",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Categories_CategoryId",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "News",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "News",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "News",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "News",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_News_CategoryId",
                table: "News",
                newName: "IX_News_categoryId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Categories",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Categories",
                newName: "description");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Categories_categoryId",
                table: "News",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
