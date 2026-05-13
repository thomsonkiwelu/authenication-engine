using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParkColumnInOfficeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Offices",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<Guid>(
                name: "ParkId",
                table: "Offices",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_ParkId",
                table: "Offices",
                column: "ParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Parks_ParkId",
                table: "Offices",
                column: "ParkId",
                principalTable: "Parks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Parks_ParkId",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_ParkId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "ParkId",
                table: "Offices");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Offices",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
