using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficeIdInUnitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OfficeId",
                table: "Units",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Units_OfficeId",
                table: "Units",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Offices_OfficeId",
                table: "Units",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Offices_OfficeId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_OfficeId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "Units");
        }
    }
}
