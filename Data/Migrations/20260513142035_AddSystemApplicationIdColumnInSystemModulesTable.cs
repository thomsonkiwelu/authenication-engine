using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication_engine.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemApplicationIdColumnInSystemModulesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SystemApplicationId",
                table: "SystemModules",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemModules_SystemApplicationId",
                table: "SystemModules",
                column: "SystemApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModules_SystemApplications_SystemApplicationId",
                table: "SystemModules",
                column: "SystemApplicationId",
                principalTable: "SystemApplications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemModules_SystemApplications_SystemApplicationId",
                table: "SystemModules");

            migrationBuilder.DropIndex(
                name: "IX_SystemModules_SystemApplicationId",
                table: "SystemModules");

            migrationBuilder.DropColumn(
                name: "SystemApplicationId",
                table: "SystemModules");
        }
    }
}
