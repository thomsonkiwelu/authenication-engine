using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficeScopeToLessRangerDailyDivisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessRangerDailyDivisions_Parks_ParkId",
                table: "LessRangerDailyDivisions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParkId",
                table: "LessRangerDailyDivisions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "OfficeId",
                table: "LessRangerDailyDivisions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessRangerDailyDivisions_OfficeId",
                table: "LessRangerDailyDivisions",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessRangerDailyDivisions_Offices_OfficeId",
                table: "LessRangerDailyDivisions",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessRangerDailyDivisions_Parks_ParkId",
                table: "LessRangerDailyDivisions",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessRangerDailyDivisions_Offices_OfficeId",
                table: "LessRangerDailyDivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_LessRangerDailyDivisions_Parks_ParkId",
                table: "LessRangerDailyDivisions");

            migrationBuilder.DropIndex(
                name: "IX_LessRangerDailyDivisions_OfficeId",
                table: "LessRangerDailyDivisions");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "LessRangerDailyDivisions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParkId",
                table: "LessRangerDailyDivisions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessRangerDailyDivisions_Parks_ParkId",
                table: "LessRangerDailyDivisions",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
