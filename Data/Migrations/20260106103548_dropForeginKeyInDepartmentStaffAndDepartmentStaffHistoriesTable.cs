using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class dropForeginKeyInDepartmentStaffAndDepartmentStaffHistoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentStaffHistories_Departments_DepartmentId",
                table: "DepartmentStaffHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentStaffs_Departments_DepartmentId",
                table: "DepartmentStaffs");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentStaffHistories_DepartmentId",
                table: "DepartmentStaffHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DepartmentStaffHistories_DepartmentId",
                table: "DepartmentStaffHistories",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentStaffHistories_Departments_DepartmentId",
                table: "DepartmentStaffHistories",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentStaffs_Departments_DepartmentId",
                table: "DepartmentStaffs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
