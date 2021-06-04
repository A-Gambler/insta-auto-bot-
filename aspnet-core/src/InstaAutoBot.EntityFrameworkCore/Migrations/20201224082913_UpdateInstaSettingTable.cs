using Microsoft.EntityFrameworkCore.Migrations;

namespace InstaAutoBot.Migrations
{
    public partial class UpdateInstaSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageNumbers",
                table: "InstaSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PostNumbers",
                table: "InstaSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoryNumbers",
                table: "InstaSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageNumbers",
                table: "InstaSettings");

            migrationBuilder.DropColumn(
                name: "PostNumbers",
                table: "InstaSettings");

            migrationBuilder.DropColumn(
                name: "StoryNumbers",
                table: "InstaSettings");
        }
    }
}
