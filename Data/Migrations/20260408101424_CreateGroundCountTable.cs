using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateGroundCountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroundCounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalAreaNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransectId = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransectStartingPoint = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    TransectEndPoint = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    EndDistance = table.Column<float>(type: "real", nullable: false),
                    RouteDescription = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    WeatherCondition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OtherWeatherCondition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroundCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroundCounts_Locations_LocalAreaNameId",
                        column: x => x.LocalAreaNameId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroundCounts_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroundCounts_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroundCounts_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroundCounts_CreatedBy",
                table: "GroundCounts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GroundCounts_LocalAreaNameId",
                table: "GroundCounts",
                column: "LocalAreaNameId");

            migrationBuilder.CreateIndex(
                name: "IX_GroundCounts_ParkId",
                table: "GroundCounts",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_GroundCounts_UpdatedBy",
                table: "GroundCounts",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroundCounts");
        }
    }
}
