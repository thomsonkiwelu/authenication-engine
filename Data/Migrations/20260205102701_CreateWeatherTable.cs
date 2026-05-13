using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateWeatherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TmaRegistrationNumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CollectionMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RainfallFrequency = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Rainfall = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MinimumTemperature = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MaximumTemperature = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MeanTemperature = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    WindDirection = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    WindSpeed = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AverageWindSpeed = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DryHumidity = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    WetHumidity = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AverageHumidity = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CloudCover = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Sunshine = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Pressure = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Evaporation = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Radiation = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DeviceName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Coordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weather_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Weather_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Weather_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weather_CreatedBy",
                table: "Weather",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Weather_StationId",
                table: "Weather",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Weather_UpdatedBy",
                table: "Weather",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weather");
        }
    }
}
