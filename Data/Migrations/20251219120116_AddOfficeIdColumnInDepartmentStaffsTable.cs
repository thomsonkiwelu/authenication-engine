using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficeIdColumnInDepartmentStaffsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OfficeId",
                table: "DepartmentStaffs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentStaffs_OfficeId",
                table: "DepartmentStaffs",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentStaffs_Offices_OfficeId",
                table: "DepartmentStaffs",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentStaffs_Offices_OfficeId",
                table: "DepartmentStaffs");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentStaffs_OfficeId",
                table: "DepartmentStaffs");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "DepartmentStaffs");
        }
    }
}
