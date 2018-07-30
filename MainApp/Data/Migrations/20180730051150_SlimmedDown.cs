using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MainApp.Data.Migrations
{
    public partial class SlimmedDown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Framework_GenericAttributes");

            migrationBuilder.DropTable(
                name: "Framework_Languages");

            migrationBuilder.DropTable(
                name: "Framework_LocalizableProperties");

            migrationBuilder.DropTable(
                name: "Framework_LocalizableStrings");

            migrationBuilder.DropTable(
                name: "Framework_Log");

            migrationBuilder.DropTable(
                name: "Framework_ScheduledTasks");

            migrationBuilder.DropTable(
                name: "Framework_Settings");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Framework_GenericAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntityId = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    EntityType = table.Column<string>(unicode: false, maxLength: 512, nullable: false),
                    Property = table.Column<string>(unicode: false, maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framework_GenericAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Framework_Languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CultureCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    IsRTL = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framework_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Framework_LocalizableProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CultureCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    EntityId = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    EntityType = table.Column<string>(unicode: false, maxLength: 512, nullable: false),
                    Property = table.Column<string>(unicode: false, maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framework_LocalizableProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Framework_LocalizableStrings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CultureCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    TextKey = table.Column<string>(nullable: false),
                    TextValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framework_LocalizableStrings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Framework_Log",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ErrorClass = table.Column<string>(unicode: false, maxLength: 512, nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    ErrorMethod = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    ErrorSource = table.Column<string>(maxLength: 255, nullable: false),
                    EventDateTime = table.Column<DateTime>(nullable: false),
                    EventLevel = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    EventMessage = table.Column<string>(nullable: false),
                    InnerErrorMessage = table.Column<string>(nullable: true),
                    MachineName = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framework_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Framework_ScheduledTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>(nullable: false),
                    LastEndUtc = table.Column<DateTime>(nullable: true),
                    LastStartUtc = table.Column<DateTime>(nullable: true),
                    LastSuccessUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Seconds = table.Column<int>(nullable: false),
                    StopOnError = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framework_ScheduledTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Framework_Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Type = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Framework_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(maxLength: 255, nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.PermissionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");
        }
    }
}
