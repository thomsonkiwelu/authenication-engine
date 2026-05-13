using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateWaterQualityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterQualities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WaterQualityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WaterBodyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    AtmosphericPressure = table.Column<float>(type: "real", nullable: false),
                    OxidationReductionPotential = table.Column<float>(type: "real", nullable: false),
                    DissolvedOxygenInPercentage = table.Column<float>(type: "real", nullable: false),
                    DissolvedOxygenInMg = table.Column<float>(type: "real", nullable: false),
                    TotalDissolvedSolid = table.Column<float>(type: "real", nullable: false),
                    Resistivity = table.Column<float>(type: "real", nullable: false),
                    SalinityInPpt = table.Column<float>(type: "real", nullable: false),
                    SalinityInPercentage = table.Column<float>(type: "real", nullable: false),
                    Ssg = table.Column<float>(type: "real", nullable: false),
                    WaterFlowRate = table.Column<float>(type: "real", nullable: false),
                    FecalColiform = table.Column<float>(type: "real", nullable: false),
                    TotalColiform = table.Column<float>(type: "real", nullable: false),
                    PotentialOfHydrogen = table.Column<float>(type: "real", nullable: false),
                    ElectricConductivity = table.Column<float>(type: "real", nullable: false),
                    Nitrate = table.Column<float>(type: "real", nullable: false),
                    Fluoride = table.Column<float>(type: "real", nullable: false),
                    Chloride = table.Column<float>(type: "real", nullable: false),
                    TotalAlkalinity = table.Column<float>(type: "real", nullable: false),
                    Phosphate = table.Column<float>(type: "real", nullable: false),
                    Turbidity = table.Column<float>(type: "real", nullable: false),
                    Color = table.Column<float>(type: "real", nullable: false),
                    Settleable = table.Column<float>(type: "real", nullable: false),
                    TotalHardness = table.Column<float>(type: "real", nullable: false),
                    Calcium = table.Column<float>(type: "real", nullable: false),
                    Magnesium = table.Column<float>(type: "real", nullable: false),
                    Iron = table.Column<float>(type: "real", nullable: false),
                    Copper = table.Column<float>(type: "real", nullable: false),
                    Chromium = table.Column<float>(type: "real", nullable: false),
                    Ammonia = table.Column<float>(type: "real", nullable: false),
                    Nitrite = table.Column<float>(type: "real", nullable: false),
                    Sulphate = table.Column<float>(type: "real", nullable: false),
                    Sodium = table.Column<float>(type: "real", nullable: false),
                    Potassium = table.Column<float>(type: "real", nullable: false),
                    TotalSuspendedSolid = table.Column<float>(type: "real", nullable: false),
                    Coordinate = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterQualities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterQualities_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WaterQualities_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaterQualities_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaterQualities_WaterBodies_WaterBodyId",
                        column: x => x.WaterBodyId,
                        principalTable: "WaterBodies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterQualities_CreatedBy",
                table: "WaterQualities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WaterQualities_ParkId",
                table: "WaterQualities",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterQualities_UpdatedBy",
                table: "WaterQualities",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WaterQualities_WaterBodyId",
                table: "WaterQualities",
                column: "WaterBodyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterQualities");
        }
    }
}
