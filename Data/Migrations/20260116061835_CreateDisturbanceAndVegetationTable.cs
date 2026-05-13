using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateDisturbanceAndVegetationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vegetations");
            
            migrationBuilder.CreateTable(
                name: "Disturbances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disturbances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disturbances_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Disturbances_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vegetations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Session = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Rainfall = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Temperature = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Altitude = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Slope = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SoilType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VegetationZone = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    VegetationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Coordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Methodology = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LifeForm = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FamilyName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uuid", maxLength: 150, nullable: false),
                    Height = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Weight = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StemNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Diameter = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Cover = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    DiameterAtBreastHeight = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Circumference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CanopyDiameter = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CanopyClosure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PlotId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    VegetationCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartCoordinate = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    EndCoordinate = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vegetations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vegetations_Locations_LocalNameId",
                        column: x => x.LocalNameId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vegetations_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vegetations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vegetations_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disturbances_CreatedBy",
                table: "Disturbances",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Disturbances_UpdatedBy",
                table: "Disturbances",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Vegetations_CreatedBy",
                table: "Vegetations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Vegetations_LocalNameId",
                table: "Vegetations",
                column: "LocalNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Vegetations_SpeciesId",
                table: "Vegetations",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Vegetations_UpdatedBy",
                table: "Vegetations",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Disturbances");

            migrationBuilder.DropTable(
                name: "Vegetations");
        }
    }
}
