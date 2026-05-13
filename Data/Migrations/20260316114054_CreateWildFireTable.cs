using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateWildFireTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WildFires",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalAreaNameId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceOfFire = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BurntArea = table.Column<float>(type: "real", nullable: false),
                    BurningDuration = table.Column<float>(type: "real", nullable: false),
                    ParticipantStaff = table.Column<float>(type: "real", nullable: false),
                    OtherParticipant = table.Column<float>(type: "real", nullable: false),
                    OtherFireSource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_WildFires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WildFires_Locations_LocalAreaNameId",
                        column: x => x.LocalAreaNameId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WildFires_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WildFires_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WildFires_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WildFires_CreatedBy",
                table: "WildFires",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WildFires_LocalAreaNameId",
                table: "WildFires",
                column: "LocalAreaNameId");

            migrationBuilder.CreateIndex(
                name: "IX_WildFires_ParkId",
                table: "WildFires",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_WildFires_UpdatedBy",
                table: "WildFires",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WildFires");
        }
    }
}
