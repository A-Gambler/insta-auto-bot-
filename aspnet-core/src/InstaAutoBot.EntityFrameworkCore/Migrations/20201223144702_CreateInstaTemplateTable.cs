using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstaAutoBot.Migrations
{
    public partial class CreateInstaTemplateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstaTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstaAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ZipFileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PostsIntervalInHours = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    StoriesIntervalInHours = table.Column<int>(type: "int", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_InstaTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstaTemplates_InstaAccounts_InstaAccountId",
                        column: x => x.InstaAccountId,
                        principalTable: "InstaAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstaTemplates_InstaAccountId",
                table: "InstaTemplates",
                column: "InstaAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstaTemplates");
        }
    }
}
