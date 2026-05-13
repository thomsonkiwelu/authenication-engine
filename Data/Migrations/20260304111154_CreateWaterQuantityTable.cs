using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateWaterQuantityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterQuantities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WaterBodyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaterLevel = table.Column<float>(type: "real", nullable: false),
                    WettedPerimeter = table.Column<float>(type: "real", nullable: false),
                    WettedWidth = table.Column<float>(type: "real", nullable: false),
                    AverageDepth = table.Column<float>(type: "real", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false),
                    AverageTime = table.Column<float>(type: "real", nullable: false),
                    MinimumFlowRate = table.Column<float>(type: "real", nullable: false),
                    MaximumFlowRate = table.Column<float>(type: "real", nullable: false),
                    AverageFlowRate = table.Column<float>(type: "real", nullable: false),
                    CalculatedDischargeRate = table.Column<float>(type: "real", nullable: false),
                    Volume = table.Column<float>(type: "real", nullable: false),
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
                    table.PrimaryKey("PK_WaterQuantities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterQuantities_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WaterQuantities_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaterQuantities_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaterQuantities_WaterBodies_WaterBodyId",
                        column: x => x.WaterBodyId,
                        principalTable: "WaterBodies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterQuantities_CreatedBy",
                table: "WaterQuantities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WaterQuantities_ParkId",
                table: "WaterQuantities",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterQuantities_UpdatedBy",
                table: "WaterQuantities",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WaterQuantities_WaterBodyId",
                table: "WaterQuantities",
                column: "WaterBodyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterQuantities");
        }
    }
}
