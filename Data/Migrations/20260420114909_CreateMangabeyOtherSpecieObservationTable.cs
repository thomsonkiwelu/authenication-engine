using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMangabeyOtherSpecieObservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MangabeyOtherSpecieObservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActivityObserved = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumberOfAnimalSeen = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MangabeyBehavior = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MangabeyObservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangabeyOtherSpecieObservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangabeyOtherSpecieObservations_MangabeyObservations_Mangab~",
                        column: x => x.MangabeyObservationId,
                        principalTable: "MangabeyObservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangabeyOtherSpecieObservations_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangabeyOtherSpecieObservations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MangabeyOtherSpecieObservations_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyOtherSpecieObservations_CreatedBy",
                table: "MangabeyOtherSpecieObservations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyOtherSpecieObservations_MangabeyObservationId",
                table: "MangabeyOtherSpecieObservations",
                column: "MangabeyObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyOtherSpecieObservations_SpeciesId",
                table: "MangabeyOtherSpecieObservations",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyOtherSpecieObservations_UpdatedBy",
                table: "MangabeyOtherSpecieObservations",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangabeyOtherSpecieObservations");
        }
    }
}
