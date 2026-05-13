using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateBirdSurveyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BirdSurveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalAreaNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    Method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Habitat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Session = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Coordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdSurveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BirdSurveys_Locations_LocalAreaNameId",
                        column: x => x.LocalAreaNameId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BirdSurveys_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BirdSurveys_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BirdSurveys_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BirdSurveys_CreatedBy",
                table: "BirdSurveys",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BirdSurveys_LocalAreaNameId",
                table: "BirdSurveys",
                column: "LocalAreaNameId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdSurveys_ParkId",
                table: "BirdSurveys",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdSurveys_UpdatedBy",
                table: "BirdSurveys",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BirdSurveys");
        }
    }
}
