using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemModuleColumnInPermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Module",
                table: "Permissions");

            migrationBuilder.AddColumn<Guid>(
                name: "SystemModuleId",
                table: "Permissions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_SystemModuleId",
                table: "Permissions",
                column: "SystemModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_SystemModules_SystemModuleId",
                table: "Permissions",
                column: "SystemModuleId",
                principalTable: "SystemModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_SystemModules_SystemModuleId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_SystemModuleId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "SystemModuleId",
                table: "Permissions");

            migrationBuilder.AddColumn<int>(
                name: "Module",
                table: "Permissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
