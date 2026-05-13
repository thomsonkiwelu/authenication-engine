using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficeScopeToLessStationsAndPostings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessRangerStations_LessOperationalZones_LessOperationalZone~",
                table: "LessRangerStations");

            migrationBuilder.DropForeignKey(
                name: "FK_LessStaffPostings_LessOperationalZones_LessOperationalZoneId",
                table: "LessStaffPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_LessStaffPostings_Parks_ParkId",
                table: "LessStaffPostings");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParkId",
                table: "LessStaffPostings",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "LessOperationalZoneId",
                table: "LessStaffPostings",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "OfficeId",
                table: "LessStaffPostings",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LessOperationalZoneId",
                table: "LessRangerStations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "OfficeId",
                table: "LessRangerStations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_OfficeId",
                table: "LessStaffPostings",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerStations_OfficeId",
                table: "LessRangerStations",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessRangerStations_LessOperationalZones_LessOperationalZone~",
                table: "LessRangerStations",
                column: "LessOperationalZoneId",
                principalTable: "LessOperationalZones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessRangerStations_Offices_OfficeId",
                table: "LessRangerStations",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessStaffPostings_LessOperationalZones_LessOperationalZoneId",
                table: "LessStaffPostings",
                column: "LessOperationalZoneId",
                principalTable: "LessOperationalZones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessStaffPostings_Offices_OfficeId",
                table: "LessStaffPostings",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessStaffPostings_Parks_ParkId",
                table: "LessStaffPostings",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessRangerStations_LessOperationalZones_LessOperationalZone~",
                table: "LessRangerStations");

            migrationBuilder.DropForeignKey(
                name: "FK_LessRangerStations_Offices_OfficeId",
                table: "LessRangerStations");

            migrationBuilder.DropForeignKey(
                name: "FK_LessStaffPostings_LessOperationalZones_LessOperationalZoneId",
                table: "LessStaffPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_LessStaffPostings_Offices_OfficeId",
                table: "LessStaffPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_LessStaffPostings_Parks_ParkId",
                table: "LessStaffPostings");

            migrationBuilder.DropIndex(
                name: "IX_LessStaffPostings_OfficeId",
                table: "LessStaffPostings");

            migrationBuilder.DropIndex(
                name: "IX_LessRangerStations_OfficeId",
                table: "LessRangerStations");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "LessStaffPostings");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "LessRangerStations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParkId",
                table: "LessStaffPostings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LessOperationalZoneId",
                table: "LessStaffPostings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LessOperationalZoneId",
                table: "LessRangerStations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessRangerStations_LessOperationalZones_LessOperationalZone~",
                table: "LessRangerStations",
                column: "LessOperationalZoneId",
                principalTable: "LessOperationalZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessStaffPostings_LessOperationalZones_LessOperationalZoneId",
                table: "LessStaffPostings",
                column: "LessOperationalZoneId",
                principalTable: "LessOperationalZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessStaffPostings_Parks_ParkId",
                table: "LessStaffPostings",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
