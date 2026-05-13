using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLessLivestockDaily : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessLivestockDailies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: true),
                    OfficeId = table.Column<Guid>(type: "uuid", nullable: true),
                    LessRangerStationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DutyDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessLivestockDailies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessLivestockDailies_LessRangerStations_LessRangerStationId",
                        column: x => x.LessRangerStationId,
                        principalTable: "LessRangerStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessLivestockDailies_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LessLivestockDailies_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LessLivestockDailies_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessLivestockDailies_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessLivestockDailyActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessLivestockDailyId = table.Column<Guid>(type: "uuid", nullable: false),
                    LivestockTypeKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ActionOptionKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    ControlNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CaseNumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessLivestockDailyActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessLivestockDailyActions_LessLivestockDailies_LessLivestoc~",
                        column: x => x.LessLivestockDailyId,
                        principalTable: "LessLivestockDailies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessLivestockDailyActions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessLivestockDailyActions_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessLivestockDailyLivestock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LessLivestockDailyId = table.Column<Guid>(type: "uuid", nullable: false),
                    LivestockTypeKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessLivestockDailyLivestock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessLivestockDailyLivestock_LessLivestockDailies_LessLivest~",
                        column: x => x.LessLivestockDailyId,
                        principalTable: "LessLivestockDailies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessLivestockDailyLivestock_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessLivestockDailyLivestock_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailies_CreatedBy",
                table: "LessLivestockDailies",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailies_LessRangerStationId_DutyDate",
                table: "LessLivestockDailies",
                columns: new[] { "LessRangerStationId", "DutyDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailies_OfficeId",
                table: "LessLivestockDailies",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailies_ParkId",
                table: "LessLivestockDailies",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailies_UpdatedBy",
                table: "LessLivestockDailies",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailyActions_CreatedBy",
                table: "LessLivestockDailyActions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailyActions_LessLivestockDailyId",
                table: "LessLivestockDailyActions",
                column: "LessLivestockDailyId");

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailyActions_UpdatedBy",
                table: "LessLivestockDailyActions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailyLivestock_CreatedBy",
                table: "LessLivestockDailyLivestock",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailyLivestock_LessLivestockDailyId",
                table: "LessLivestockDailyLivestock",
                column: "LessLivestockDailyId");

            migrationBuilder.CreateIndex(
                name: "IX_LessLivestockDailyLivestock_UpdatedBy",
                table: "LessLivestockDailyLivestock",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessLivestockDailyActions");

            migrationBuilder.DropTable(
                name: "LessLivestockDailyLivestock");

            migrationBuilder.DropTable(
                name: "LessLivestockDailies");
        }
    }
}
