using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateRareSpeciesOccurrenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RareSpeciesOccurrences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalAreaNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    RareEndangeredSpeciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    VegetationCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NumberOfIndividual = table.Column<float>(type: "real", nullable: false),
                    Remark = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Coordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RareSpeciesOccurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RareSpeciesOccurrences_Locations_LocalAreaNameId",
                        column: x => x.LocalAreaNameId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RareSpeciesOccurrences_RareEndangeredSpecies_RareEndangered~",
                        column: x => x.RareEndangeredSpeciesId,
                        principalTable: "RareEndangeredSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RareSpeciesOccurrences_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RareSpeciesOccurrences_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RareSpeciesOccurrences_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RareSpeciesOccurrences_CreatedBy",
                table: "RareSpeciesOccurrences",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RareSpeciesOccurrences_LocalAreaNameId",
                table: "RareSpeciesOccurrences",
                column: "LocalAreaNameId");

            migrationBuilder.CreateIndex(
                name: "IX_RareSpeciesOccurrences_RareEndangeredSpeciesId",
                table: "RareSpeciesOccurrences",
                column: "RareEndangeredSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_RareSpeciesOccurrences_SpeciesId",
                table: "RareSpeciesOccurrences",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_RareSpeciesOccurrences_UpdatedBy",
                table: "RareSpeciesOccurrences",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RareSpeciesOccurrences");
        }
    }
}
