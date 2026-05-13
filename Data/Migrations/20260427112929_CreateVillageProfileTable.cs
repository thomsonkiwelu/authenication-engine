using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateVillageProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VillageProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VillageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    VillageSize = table.Column<float>(type: "real", nullable: false),
                    NumberOfHousehold = table.Column<float>(type: "real", nullable: false),
                    NumberOfMale = table.Column<float>(type: "real", nullable: false),
                    NumberOfFemale = table.Column<float>(type: "real", nullable: false),
                    NumberOfCow = table.Column<float>(type: "real", nullable: false),
                    NumberOfSheep = table.Column<float>(type: "real", nullable: false),
                    NumberOfGoat = table.Column<float>(type: "real", nullable: false),
                    NumberOfDog = table.Column<float>(type: "real", nullable: false),
                    Population = table.Column<float>(type: "real", nullable: false),
                    LandStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillageProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VillageProfiles_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VillageProfiles_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VillageProfiles_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VillageProfiles_Villages_VillageId",
                        column: x => x.VillageId,
                        principalTable: "Villages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VillageProfiles_CreatedBy",
                table: "VillageProfiles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VillageProfiles_ParkId",
                table: "VillageProfiles",
                column: "ParkId");

            migrationBuilder.CreateIndex(
                name: "IX_VillageProfiles_UpdatedBy",
                table: "VillageProfiles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VillageProfiles_VillageId",
                table: "VillageProfiles",
                column: "VillageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillageProfiles");
        }
    }
}
