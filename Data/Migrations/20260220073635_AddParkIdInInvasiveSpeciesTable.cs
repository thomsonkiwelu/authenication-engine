using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParkIdInInvasiveSpeciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParkId",
                table: "InvasiveSpecies",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvasiveSpecies_ParkId",
                table: "InvasiveSpecies",
                column: "ParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvasiveSpecies_Parks_ParkId",
                table: "InvasiveSpecies",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvasiveSpecies_Parks_ParkId",
                table: "InvasiveSpecies");

            migrationBuilder.DropIndex(
                name: "IX_InvasiveSpecies_ParkId",
                table: "InvasiveSpecies");

            migrationBuilder.DropColumn(
                name: "ParkId",
                table: "InvasiveSpecies");
        }
    }
}
