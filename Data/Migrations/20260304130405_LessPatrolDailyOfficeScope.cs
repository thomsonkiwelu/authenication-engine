using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class LessPatrolDailyOfficeScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessPatrolDailies_Parks_ParkId",
                table: "LessPatrolDailies");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParkId",
                table: "LessPatrolDailies",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "OfficeId",
                table: "LessPatrolDailies",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessPatrolDailies_OfficeId",
                table: "LessPatrolDailies",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessPatrolDailies_Offices_OfficeId",
                table: "LessPatrolDailies",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessPatrolDailies_Parks_ParkId",
                table: "LessPatrolDailies",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessPatrolDailies_Offices_OfficeId",
                table: "LessPatrolDailies");

            migrationBuilder.DropForeignKey(
                name: "FK_LessPatrolDailies_Parks_ParkId",
                table: "LessPatrolDailies");

            migrationBuilder.DropIndex(
                name: "IX_LessPatrolDailies_OfficeId",
                table: "LessPatrolDailies");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "LessPatrolDailies");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParkId",
                table: "LessPatrolDailies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessPatrolDailies_Parks_ParkId",
                table: "LessPatrolDailies",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
