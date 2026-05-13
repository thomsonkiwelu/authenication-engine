using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDistrictProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistrictProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    AreaSize = table.Column<float>(type: "real", nullable: false),
                    Population = table.Column<float>(type: "real", nullable: false),
                    PopulationGrowthRate = table.Column<float>(type: "real", nullable: false),
                    AreaOccupiedByPark = table.Column<float>(type: "real", nullable: false),
                    RelationshipStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AverageAnnualRainfall = table.Column<float>(type: "real", nullable: false),
                    Landform = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    RainfallPattern = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    VegetationCharacteristics = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistrictProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistrictProfiles_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistrictProfiles_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistrictProfiles_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DistrictProfiles_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistrictProfiles_CreatedBy",
                table: "DistrictProfiles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DistrictProfiles_DistrictId",
                table: "DistrictProfiles",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_DistrictProfiles_ParkId",
                table: "DistrictProfiles",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_DistrictProfiles_UpdatedBy",
                table: "DistrictProfiles",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistrictProfiles");
        }
    }
}
