using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueActiveLessStaffPostingPerStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LessStaffPostings_StaffId",
                table: "LessStaffPostings");

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_StaffId",
                table: "LessStaffPostings",
                column: "StaffId",
                unique: true,
                filter: "\"EffectiveTo\" IS NULL AND \"DeletedAt\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LessStaffPostings_StaffId",
                table: "LessStaffPostings");

            migrationBuilder.CreateIndex(
                name: "IX_LessStaffPostings_StaffId",
                table: "LessStaffPostings",
                column: "StaffId");
        }
    }
}
