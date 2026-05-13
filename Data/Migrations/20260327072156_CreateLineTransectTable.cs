using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateLineTransectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LineTransects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    Session = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LineTransectStartCoordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    LineTransectRecordAltitude = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    LineTransectHabitat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LineTransectLocalAreaNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlongTransectCoordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AlongTransectRecordAltitude = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AlongTransectHabitat = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    AlongTransectLocalAreaNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    EndTransectCoordinates = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    EndTransectRecordAltitude = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineTransects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineTransects_Locations_AlongTransectLocalAreaNameId",
                        column: x => x.AlongTransectLocalAreaNameId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineTransects_Locations_LineTransectLocalAreaNameId",
                        column: x => x.LineTransectLocalAreaNameId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineTransects_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineTransects_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineTransects_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineTransects_AlongTransectLocalAreaNameId",
                table: "LineTransects",
                column: "AlongTransectLocalAreaNameId");

            migrationBuilder.CreateIndex(
                name: "IX_LineTransects_CreatedBy",
                table: "LineTransects",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LineTransects_LineTransectLocalAreaNameId",
                table: "LineTransects",
                column: "LineTransectLocalAreaNameId");

            migrationBuilder.CreateIndex(
                name: "IX_LineTransects_ParkId",
                table: "LineTransects",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_LineTransects_UpdatedBy",
                table: "LineTransects",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineTransects");
        }
    }
}
