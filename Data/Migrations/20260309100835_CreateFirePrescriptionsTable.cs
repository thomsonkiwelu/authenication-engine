using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateFirePrescriptionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FirePrescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalAreaNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkArea = table.Column<float>(type: "real", nullable: false),
                    PlannedArea = table.Column<float>(type: "real", nullable: false),
                    ActualBurntArea = table.Column<float>(type: "real", nullable: false),
                    Coordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirePrescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirePrescriptions_Locations_LocalAreaNameId",
                        column: x => x.LocalAreaNameId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FirePrescriptions_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FirePrescriptions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FirePrescriptions_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FirePrescriptions_CreatedBy",
                table: "FirePrescriptions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FirePrescriptions_LocalAreaNameId",
                table: "FirePrescriptions",
                column: "LocalAreaNameId");

            migrationBuilder.CreateIndex(
                name: "IX_FirePrescriptions_ParkId",
                table: "FirePrescriptions",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_FirePrescriptions_UpdatedBy",
                table: "FirePrescriptions",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FirePrescriptions");
        }
    }
}
