using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParkIdInWasteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParkId",
                table: "Wastes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wastes_ParkId",
                table: "Wastes",
                column: "ParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wastes_Parks_ParkId",
                table: "Wastes",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wastes_Parks_ParkId",
                table: "Wastes");

            migrationBuilder.DropIndex(
                name: "IX_Wastes_ParkId",
                table: "Wastes");

            migrationBuilder.DropColumn(
                name: "ParkId",
                table: "Wastes");
        }
    }
}
