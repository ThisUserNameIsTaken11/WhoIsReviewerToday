using Microsoft.EntityFrameworkCore.Migrations;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Migrations
{
    public partial class UpdateForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Developers_Chats_ChatId",
                table: "Developers");

            migrationBuilder.AlterColumn<long>(
                name: "ChatId",
                table: "Developers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Developers_Chats_ChatId",
                table: "Developers",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Developers_Chats_ChatId",
                table: "Developers");

            migrationBuilder.AlterColumn<long>(
                name: "ChatId",
                table: "Developers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Developers_Chats_ChatId",
                table: "Developers",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
