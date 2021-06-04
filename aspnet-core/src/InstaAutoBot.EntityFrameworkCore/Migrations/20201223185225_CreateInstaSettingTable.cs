using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstaAutoBot.Migrations
{
    public partial class CreateInstaSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstaMessageRecipients_InstaMessages_InstaMessageId",
                table: "InstaMessageRecipients");

            migrationBuilder.RenameColumn(
                name: "InstaMessageId",
                table: "InstaMessageRecipients",
                newName: "InstaSettingId");

            migrationBuilder.RenameIndex(
                name: "IX_InstaMessageRecipients_InstaMessageId",
                table: "InstaMessageRecipients",
                newName: "IX_InstaMessageRecipients_InstaSettingId");

            migrationBuilder.CreateTable(
                name: "InstaSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostInterval = table.Column<int>(type: "int", nullable: false),
                    PostIntervalValue = table.Column<int>(type: "int", nullable: false),
                    StoryInterval = table.Column<int>(type: "int", nullable: false),
                    StoryIntervalValue = table.Column<int>(type: "int", nullable: false),
                    MessageInterval = table.Column<int>(type: "int", nullable: false),
                    MessageIntervalValue = table.Column<int>(type: "int", nullable: false),
                    MessageBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstaSettings", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InstaMessageRecipients_InstaSettings_InstaSettingId",
                table: "InstaMessageRecipients",
                column: "InstaSettingId",
                principalTable: "InstaSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstaMessageRecipients_InstaSettings_InstaSettingId",
                table: "InstaMessageRecipients");

            migrationBuilder.DropTable(
                name: "InstaSettings");

            migrationBuilder.RenameColumn(
                name: "InstaSettingId",
                table: "InstaMessageRecipients",
                newName: "InstaMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_InstaMessageRecipients_InstaSettingId",
                table: "InstaMessageRecipients",
                newName: "IX_InstaMessageRecipients_InstaMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstaMessageRecipients_InstaMessages_InstaMessageId",
                table: "InstaMessageRecipients",
                column: "InstaMessageId",
                principalTable: "InstaMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
