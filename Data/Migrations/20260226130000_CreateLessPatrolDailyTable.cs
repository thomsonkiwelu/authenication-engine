using System;
using conservation_backend.Config;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20260226130000_CreateLessPatrolDailyTable")]
    public partial class CreateLessPatrolDailyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessPatrolDailies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessRangerStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DutyDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ManDaysPlanned = table.Column<int>(type: "integer", nullable: false),
                    ManDaysPerformed = table.Column<int>(type: "integer", nullable: false),
                    FootPatrolPlanned = table.Column<int>(type: "integer", nullable: false),
                    FootPatrolPerformed = table.Column<int>(type: "integer", nullable: false),
                    VehiclePatrolPlanned = table.Column<int>(type: "integer", nullable: false),
                    VehiclePatrolPerformed = table.Column<int>(type: "integer", nullable: false),
                    BoatPatrolPlanned = table.Column<int>(type: "integer", nullable: false),
                    BoatPatrolPerformed = table.Column<int>(type: "integer", nullable: false),
                    AirPatrolPlanned = table.Column<int>(type: "integer", nullable: false),
                    AirPatrolPerformed = table.Column<int>(type: "integer", nullable: false),
                    AirPatrolHoursPlanned = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    AirPatrolHoursPerformed = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    AreaInspectedKm = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessPatrolDailies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessPatrolDailies_LessRangerStations_LessRangerStationId",
                        column: x => x.LessRangerStationId,
                        principalTable: "LessRangerStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessPatrolDailies_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessPatrolDailies_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessPatrolDailies_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessPatrolDailies_LessRangerStationId_DutyDate",
                table: "LessPatrolDailies",
                columns: new[] { "LessRangerStationId", "DutyDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessPatrolDailies_ParkId",
                table: "LessPatrolDailies",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_LessPatrolDailies_LessRangerStationId",
                table: "LessPatrolDailies",
                column: "LessRangerStationId");

            migrationBuilder.CreateIndex(
                name: "IX_LessPatrolDailies_CreatedBy",
                table: "LessPatrolDailies",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessPatrolDailies_UpdatedBy",
                table: "LessPatrolDailies",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessPatrolDailies");
        }
    }
}
