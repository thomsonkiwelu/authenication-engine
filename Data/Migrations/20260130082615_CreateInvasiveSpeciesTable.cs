using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateInvasiveSpeciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvasiveSpecies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalAreaNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActivityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ControlType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    InfestedArea = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    AreaCoverage = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    BiologicalMethod = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    BiologicalAgent = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    OtherPossibleSource = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Coordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvasiveSpecies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvasiveSpecies_Locations_LocalAreaNameId",
                        column: x => x.LocalAreaNameId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvasiveSpecies_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvasiveSpecies_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvasiveSpecies_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvasiveSpecies_CreatedBy",
                table: "InvasiveSpecies",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InvasiveSpecies_LocalAreaNameId",
                table: "InvasiveSpecies",
                column: "LocalAreaNameId");

            migrationBuilder.CreateIndex(
                name: "IX_InvasiveSpecies_SpeciesId",
                table: "InvasiveSpecies",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_InvasiveSpecies_UpdatedBy",
                table: "InvasiveSpecies",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvasiveSpecies");
        }
    }
}
