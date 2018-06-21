using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FrameworkDemo.Data.Migrations
{
    public partial class Changes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Framework_MessageTemplates");

            migrationBuilder.CreateTable(
                name: "Framework_MessageTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Editor = table.Column<string>(maxLength: 255, nullable: false),
                    OwnerId = table.Column<Guid>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantle_MessageTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Framework_MessageTemplateVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CultureCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    Data = table.Column<string>(nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    DateModifiedUtc = table.Column<DateTime>(nullable: false),
                    MessageTemplateId = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framework_MessageTemplateVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Framework_MessageTemplateVersions_Framework_MessageTemplates_MessageTemplateId",
                        column: x => x.MessageTemplateId,
                        principalTable: "Framework_MessageTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Framework_MessageTemplateVersions_MessageTemplateId",
                table: "Framework_MessageTemplateVersions",
                column: "MessageTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Framework_MessageTemplateVersions");

            migrationBuilder.DropTable(name: "Framework_MessageTemplates");

            migrationBuilder.CreateTable(
                name: "Framework_MessageTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Body = table.Column<string>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    OwnerId = table.Column<Guid>(nullable: true),
                    Subject = table.Column<string>(maxLength: 255, nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantle_MessageTemplates", x => x.Id);
                });
        }
    }
}