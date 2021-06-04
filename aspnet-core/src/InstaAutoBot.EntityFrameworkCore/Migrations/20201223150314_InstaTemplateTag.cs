using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstaAutoBot.Migrations
{
    public partial class InstaTemplateTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstaTemplateTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstaTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    TagUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
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
                    table.PrimaryKey("PK_InstaTemplateTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstaTemplateTags_InstaTemplates_InstaTemplateId",
                        column: x => x.InstaTemplateId,
                        principalTable: "InstaTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstaTemplateTags_InstaTemplateId",
                table: "InstaTemplateTags",
                column: "InstaTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstaTemplateTags");
        }
    }
}
