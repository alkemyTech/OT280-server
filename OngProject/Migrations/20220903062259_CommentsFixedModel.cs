using Microsoft.EntityFrameworkCore.Migrations;

namespace OngProject.Migrations
{
    public partial class CommentsFixedModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_News_NewId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Comments",
                newName: "body");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Comments",
                newName: "news_id");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                newName: "IX_Comments_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_NewId",
                table: "Comments",
                newName: "IX_Comments_news_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_user_id",
                table: "Comments",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_News_news_id",
                table: "Comments",
                column: "news_id",
                principalTable: "News",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_user_id",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_News_news_id",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "body",
                table: "Comments",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "news_id",
                table: "Comments",
                newName: "NewId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_user_id",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_news_id",
                table: "Comments",
                newName: "IX_Comments_NewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_News_NewId",
                table: "Comments",
                column: "NewId",
                principalTable: "News",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
