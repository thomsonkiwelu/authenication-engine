using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalAreaNameInRoadKillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocalAreaNameId",
                table: "RoadKills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RoadKills_LocalAreaNameId",
                table: "RoadKills",
                column: "LocalAreaNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoadKills_Locations_LocalAreaNameId",
                table: "RoadKills",
                column: "LocalAreaNameId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoadKills_Locations_LocalAreaNameId",
                table: "RoadKills");

            migrationBuilder.DropIndex(
                name: "IX_RoadKills_LocalAreaNameId",
                table: "RoadKills");

            migrationBuilder.DropColumn(
                name: "LocalAreaNameId",
                table: "RoadKills");
        }
    }
}
