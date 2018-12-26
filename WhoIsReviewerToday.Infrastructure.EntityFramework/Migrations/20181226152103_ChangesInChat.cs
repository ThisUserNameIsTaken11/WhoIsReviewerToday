using Microsoft.EntityFrameworkCore.Migrations;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Migrations
{
    public partial class ChangesInChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Chats");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Chats",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Chats",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Chats");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Chats",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Chats",
                nullable: true);
        }
    }
}
