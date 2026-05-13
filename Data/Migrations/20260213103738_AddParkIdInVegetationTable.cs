using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParkIdInVegetationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParkId",
                table: "Vegetations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vegetations_ParkId",
                table: "Vegetations",
                column: "ParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vegetations_Parks_ParkId",
                table: "Vegetations",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vegetations_Parks_ParkId",
                table: "Vegetations");

            migrationBuilder.DropIndex(
                name: "IX_Vegetations_ParkId",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "ParkId",
                table: "Vegetations");
        }
    }
}
