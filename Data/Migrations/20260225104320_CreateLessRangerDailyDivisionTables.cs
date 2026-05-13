using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateLessRangerDailyDivisionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessRangerDailyDivisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessRangerStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DutyDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Category = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessRangerDailyDivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessRangerDailyDivisions_LessRangerStations_LessRangerStati~",
                        column: x => x.LessRangerStationId,
                        principalTable: "LessRangerStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessRangerDailyDivisions_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessRangerDailyDivisions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessRangerDailyDivisions_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessRangerDailyDivisionAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessRangerDailyDivisionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DutyFieldDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StaffId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessRangerDailyDivisionAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessRangerDailyDivisionAssignments_LessRangerDailyDivisions~",
                        column: x => x.LessRangerDailyDivisionId,
                        principalTable: "LessRangerDailyDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessRangerDailyDivisionAssignments_LessRangerDutyFieldDefin~",
                        column: x => x.DutyFieldDefinitionId,
                        principalTable: "LessRangerDutyFieldDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessRangerDailyDivisionAssignments_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessRangerDailyDivisionAssignments_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessRangerDailyDivisionAssignments_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisionAssignments_CreatedBy",
                table: "LessRangerDailyDivisionAssignments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisionAssignments_DutyFieldDefinitionId",
                table: "LessRangerDailyDivisionAssignments",
                column: "DutyFieldDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisionAssignments_LessRangerDailyDivisionI~",
                table: "LessRangerDailyDivisionAssignments",
                columns: new[] { "LessRangerDailyDivisionId", "StaffId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisionAssignments_StaffId",
                table: "LessRangerDailyDivisionAssignments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisionAssignments_UpdatedBy",
                table: "LessRangerDailyDivisionAssignments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisions_CreatedBy",
                table: "LessRangerDailyDivisions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisions_LessRangerStationId_DutyDate_Categ~",
                table: "LessRangerDailyDivisions",
                columns: new[] { "LessRangerStationId", "DutyDate", "Category" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisions_ParkId",
                table: "LessRangerDailyDivisions",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisions_UpdatedBy",
                table: "LessRangerDailyDivisions",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessRangerDailyDivisionAssignments");

            migrationBuilder.DropTable(
                name: "LessRangerDailyDivisions");
        }
    }
}
