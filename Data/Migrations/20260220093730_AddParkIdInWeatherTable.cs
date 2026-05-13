using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParkIdInWeatherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParkId",
                table: "Weather",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weather_ParkId",
                table: "Weather",
                column: "ParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weather_Parks_ParkId",
                table: "Weather",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weather_Parks_ParkId",
                table: "Weather");

            migrationBuilder.DropIndex(
                name: "IX_Weather_ParkId",
                table: "Weather");

            migrationBuilder.DropColumn(
                name: "ParkId",
                table: "Weather");
        }
    }
}
