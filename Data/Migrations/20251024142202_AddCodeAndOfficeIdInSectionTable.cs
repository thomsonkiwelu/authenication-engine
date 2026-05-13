using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeAndOfficeIdInSectionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Sections",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OfficeId",
                table: "Sections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Sections_OfficeId",
                table: "Sections",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Offices_OfficeId",
                table: "Sections",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Offices_OfficeId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_OfficeId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "Sections");
        }
    }
}
