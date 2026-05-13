using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParkIdInRoadKillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParkId",
                table: "RoadKills",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoadKills_ParkId",
                table: "RoadKills",
                column: "ParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoadKills_Parks_ParkId",
                table: "RoadKills",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoadKills_Parks_ParkId",
                table: "RoadKills");

            migrationBuilder.DropIndex(
                name: "IX_RoadKills_ParkId",
                table: "RoadKills");

            migrationBuilder.DropColumn(
                name: "ParkId",
                table: "RoadKills");
        }
    }
}
