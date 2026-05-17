using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication_engine.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemApplicationIdInPermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SystemApplicationId",
                table: "Permissions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_SystemApplicationId",
                table: "Permissions",
                column: "SystemApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_SystemApplications_SystemApplicationId",
                table: "Permissions",
                column: "SystemApplicationId",
                principalTable: "SystemApplications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_SystemApplications_SystemApplicationId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_SystemApplicationId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "SystemApplicationId",
                table: "Permissions");
        }
    }
}
