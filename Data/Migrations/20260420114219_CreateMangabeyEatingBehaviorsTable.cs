using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMangabeyEatingBehaviorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MangabeyEatingBehaviors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumedTreePart = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OtherFood = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OtherInsect = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    MangabeyObservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangabeyEatingBehaviors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangabeyEatingBehaviors_MangabeyObservations_MangabeyObserv~",
                        column: x => x.MangabeyObservationId,
                        principalTable: "MangabeyObservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangabeyEatingBehaviors_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangabeyEatingBehaviors_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MangabeyEatingBehaviors_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyEatingBehaviors_CreatedBy",
                table: "MangabeyEatingBehaviors",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyEatingBehaviors_MangabeyObservationId",
                table: "MangabeyEatingBehaviors",
                column: "MangabeyObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyEatingBehaviors_SpeciesId",
                table: "MangabeyEatingBehaviors",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyEatingBehaviors_UpdatedBy",
                table: "MangabeyEatingBehaviors",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangabeyEatingBehaviors");
        }
    }
}
