using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateLessRangerStationsAndGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessRangerStations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LessOperationalZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessRangerStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessRangerStations_LessOperationalZones_LessOperationalZone~",
                        column: x => x.LessOperationalZoneId,
                        principalTable: "LessOperationalZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessRangerStations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessRangerStations_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessRangerGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LessRangerStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessRangerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessRangerGroups_LessRangerStations_LessRangerStationId",
                        column: x => x.LessRangerStationId,
                        principalTable: "LessRangerStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessRangerGroups_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessRangerGroups_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerGroups_CreatedBy",
                table: "LessRangerGroups",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerGroups_LessRangerStationId",
                table: "LessRangerGroups",
                column: "LessRangerStationId");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerGroups_UpdatedBy",
                table: "LessRangerGroups",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerStations_CreatedBy",
                table: "LessRangerStations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerStations_LessOperationalZoneId",
                table: "LessRangerStations",
                column: "LessOperationalZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerStations_UpdatedBy",
                table: "LessRangerStations",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessRangerGroups");

            migrationBuilder.DropTable(
                name: "LessRangerStations");
        }
    }
}
