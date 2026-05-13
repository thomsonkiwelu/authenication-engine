using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication_engine.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemApplicationIdColumnInRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "SystemApplicationId",
                table: "Roles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_SystemApplicationId",
                table: "Roles",
                column: "SystemApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_SystemApplications_SystemApplicationId",
                table: "Roles",
                column: "SystemApplicationId",
                principalTable: "SystemApplications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_SystemApplications_SystemApplicationId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_SystemApplicationId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "SystemApplicationId",
                table: "Roles");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Roles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
