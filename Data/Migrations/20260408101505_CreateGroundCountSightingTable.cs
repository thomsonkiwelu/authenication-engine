using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateGroundCountSightingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroundCountSightings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroundCountId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeOfSighting = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PerpendicularDistance = table.Column<float>(type: "real", nullable: false),
                    Distance = table.Column<float>(type: "real", nullable: false),
                    Coordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroundCountSightings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroundCountSightings_GroundCounts_GroundCountId",
                        column: x => x.GroundCountId,
                        principalTable: "GroundCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroundCountSightings_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroundCountSightings_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroundCountSightings_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroundCountSightings_CreatedBy",
                table: "GroundCountSightings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GroundCountSightings_GroundCountId",
                table: "GroundCountSightings",
                column: "GroundCountId");

            migrationBuilder.CreateIndex(
                name: "IX_GroundCountSightings_SpeciesId",
                table: "GroundCountSightings",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_GroundCountSightings_UpdatedBy",
                table: "GroundCountSightings",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroundCountSightings");
        }
    }
}
