using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMangabeyObservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"AnimalImpacts\" CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Census\" CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Waters\" CASCADE;");
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"Fires\" CASCADE;");

            migrationBuilder.CreateTable(
                name: "MangabeyObservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActivityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumberOfParticipant = table.Column<float>(type: "real", nullable: false),
                    Coordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangabeyObservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangabeyObservations_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangabeyObservations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MangabeyObservations_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyObservations_CreatedBy",
                table: "MangabeyObservations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyObservations_ParkId",
                table: "MangabeyObservations",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyObservations_UpdatedBy",
                table: "MangabeyObservations",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangabeyObservations");
        }
    }
}
