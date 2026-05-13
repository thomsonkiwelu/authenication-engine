using System;
using conservation_backend.Config;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20260225020000_CreateLessStaffPostingsTable")]
    public partial class CreateLessStaffPostingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessStaffPostings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StaffId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessOperationalZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessRangerStationId = table.Column<Guid>(type: "uuid", nullable: true),
                    LessRangerGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarks = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessStaffPostings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessStaffPostings_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessStaffPostings_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessStaffPostings_LessOperationalZones_LessOperationalZoneId",
                        column: x => x.LessOperationalZoneId,
                        principalTable: "LessOperationalZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessStaffPostings_LessRangerStations_LessRangerStationId",
                        column: x => x.LessRangerStationId,
                        principalTable: "LessRangerStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessStaffPostings_LessRangerGroups_LessRangerGroupId",
                        column: x => x.LessRangerGroupId,
                        principalTable: "LessRangerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessStaffPostings_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessStaffPostings_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_StaffId",
                table: "LessStaffPostings",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_ParkId",
                table: "LessStaffPostings",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_LessOperationalZoneId",
                table: "LessStaffPostings",
                column: "LessOperationalZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_LessRangerStationId",
                table: "LessStaffPostings",
                column: "LessRangerStationId");

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_LessRangerGroupId",
                table: "LessStaffPostings",
                column: "LessRangerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_CreatedBy",
                table: "LessStaffPostings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_UpdatedBy",
                table: "LessStaffPostings",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessStaffPostings");
        }
    }
}
