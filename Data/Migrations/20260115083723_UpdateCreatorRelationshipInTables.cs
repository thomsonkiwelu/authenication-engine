using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreatorRelationshipInTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Units_CreatedBy",
                table: "Units",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Units_UpdatedBy",
                table: "Units",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModules_CreatedBy",
                table: "SystemModules",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SystemModules_UpdatedBy",
                table: "SystemModules",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Structures_CreatedBy",
                table: "Structures",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Structures_UpdatedBy",
                table: "Structures",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_CreatedBy",
                table: "Staffs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_UpdatedBy",
                table: "Staffs",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CreatedBy",
                table: "Sections",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_UpdatedBy",
                table: "Sections",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUsers_CreatedBy",
                table: "RoleUsers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUsers_UpdatedBy",
                table: "RoleUsers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedBy",
                table: "Roles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedBy",
                table: "Roles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_CreatedBy",
                table: "RolePermissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_UpdatedBy",
                table: "RolePermissions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_CreatedBy",
                table: "RefreshTokens",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UpdatedBy",
                table: "RefreshTokens",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_CreatedBy",
                table: "Ranks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_UpdatedBy",
                table: "Ranks",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CreatedBy",
                table: "Permissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UpdatedBy",
                table: "Permissions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Parks_CreatedBy",
                table: "Parks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Parks_UpdatedBy",
                table: "Parks",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_CreatedBy",
                table: "Offices",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_UpdatedBy",
                table: "Offices",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CreatedBy",
                table: "Locations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_UpdatedBy",
                table: "Locations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentStaffs_CreatedBy",
                table: "DepartmentStaffs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentStaffs_UpdatedBy",
                table: "DepartmentStaffs",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentStaffHistories_CreatedBy",
                table: "DepartmentStaffHistories",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentStaffHistories_UpdatedBy",
                table: "DepartmentStaffHistories",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedBy",
                table: "Departments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UpdatedBy",
                table: "Departments",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_CreatedBy",
                table: "Departments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_UpdatedBy",
                table: "Departments",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentStaffHistories_Users_CreatedBy",
                table: "DepartmentStaffHistories",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentStaffHistories_Users_UpdatedBy",
                table: "DepartmentStaffHistories",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentStaffs_Users_CreatedBy",
                table: "DepartmentStaffs",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentStaffs_Users_UpdatedBy",
                table: "DepartmentStaffs",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Users_CreatedBy",
                table: "Locations",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Users_UpdatedBy",
                table: "Locations",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Users_CreatedBy",
                table: "Offices",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Users_UpdatedBy",
                table: "Offices",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parks_Users_CreatedBy",
                table: "Parks",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parks_Users_UpdatedBy",
                table: "Parks",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_CreatedBy",
                table: "Permissions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_UpdatedBy",
                table: "Permissions",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ranks_Users_CreatedBy",
                table: "Ranks",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ranks_Users_UpdatedBy",
                table: "Ranks",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_CreatedBy",
                table: "RefreshTokens",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UpdatedBy",
                table: "RefreshTokens",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Users_CreatedBy",
                table: "RolePermissions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Users_UpdatedBy",
                table: "RolePermissions",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_CreatedBy",
                table: "Roles",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UpdatedBy",
                table: "Roles",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUsers_Users_CreatedBy",
                table: "RoleUsers",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUsers_Users_UpdatedBy",
                table: "RoleUsers",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Users_CreatedBy",
                table: "Sections",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Users_UpdatedBy",
                table: "Sections",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Users_CreatedBy",
                table: "Staffs",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Users_UpdatedBy",
                table: "Staffs",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Structures_Users_CreatedBy",
                table: "Structures",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Structures_Users_UpdatedBy",
                table: "Structures",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModules_Users_CreatedBy",
                table: "SystemModules",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemModules_Users_UpdatedBy",
                table: "SystemModules",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Users_CreatedBy",
                table: "Units",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Users_UpdatedBy",
                table: "Units",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_CreatedBy",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_UpdatedBy",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentStaffHistories_Users_CreatedBy",
                table: "DepartmentStaffHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentStaffHistories_Users_UpdatedBy",
                table: "DepartmentStaffHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentStaffs_Users_CreatedBy",
                table: "DepartmentStaffs");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentStaffs_Users_UpdatedBy",
                table: "DepartmentStaffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Users_CreatedBy",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Users_UpdatedBy",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Users_CreatedBy",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Users_UpdatedBy",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Parks_Users_CreatedBy",
                table: "Parks");

            migrationBuilder.DropForeignKey(
                name: "FK_Parks_Users_UpdatedBy",
                table: "Parks");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Users_CreatedBy",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Users_UpdatedBy",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Ranks_Users_CreatedBy",
                table: "Ranks");

            migrationBuilder.DropForeignKey(
                name: "FK_Ranks_Users_UpdatedBy",
                table: "Ranks");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_CreatedBy",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UpdatedBy",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Users_CreatedBy",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Users_UpdatedBy",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_CreatedBy",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UpdatedBy",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUsers_Users_CreatedBy",
                table: "RoleUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUsers_Users_UpdatedBy",
                table: "RoleUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Users_CreatedBy",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Users_UpdatedBy",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Users_CreatedBy",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Users_UpdatedBy",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Structures_Users_CreatedBy",
                table: "Structures");

            migrationBuilder.DropForeignKey(
                name: "FK_Structures_Users_UpdatedBy",
                table: "Structures");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModules_Users_CreatedBy",
                table: "SystemModules");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemModules_Users_UpdatedBy",
                table: "SystemModules");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Users_CreatedBy",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Users_UpdatedBy",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedBy",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UpdatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Units_CreatedBy",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_UpdatedBy",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_SystemModules_CreatedBy",
                table: "SystemModules");

            migrationBuilder.DropIndex(
                name: "IX_SystemModules_UpdatedBy",
                table: "SystemModules");

            migrationBuilder.DropIndex(
                name: "IX_Structures_CreatedBy",
                table: "Structures");

            migrationBuilder.DropIndex(
                name: "IX_Structures_UpdatedBy",
                table: "Structures");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_CreatedBy",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_UpdatedBy",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Sections_CreatedBy",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_UpdatedBy",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_RoleUsers_CreatedBy",
                table: "RoleUsers");

            migrationBuilder.DropIndex(
                name: "IX_RoleUsers_UpdatedBy",
                table: "RoleUsers");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CreatedBy",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UpdatedBy",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_CreatedBy",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_UpdatedBy",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_CreatedBy",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UpdatedBy",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Ranks_CreatedBy",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_Ranks_UpdatedBy",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_CreatedBy",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UpdatedBy",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Parks_CreatedBy",
                table: "Parks");

            migrationBuilder.DropIndex(
                name: "IX_Parks_UpdatedBy",
                table: "Parks");

            migrationBuilder.DropIndex(
                name: "IX_Offices_CreatedBy",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_UpdatedBy",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Locations_CreatedBy",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_UpdatedBy",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentStaffs_CreatedBy",
                table: "DepartmentStaffs");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentStaffs_UpdatedBy",
                table: "DepartmentStaffs");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentStaffHistories_CreatedBy",
                table: "DepartmentStaffHistories");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentStaffHistories_UpdatedBy",
                table: "DepartmentStaffHistories");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CreatedBy",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_UpdatedBy",
                table: "Departments");
        }
    }
}
