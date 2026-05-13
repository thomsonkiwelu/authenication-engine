using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRelationshipInSpeciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Species");
            
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Species",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Species",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Species",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Species",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_CreatedBy",
                table: "Species",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Species_UpdatedBy",
                table: "Species",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Species_Users_CreatedBy",
                table: "Species",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Species_Users_UpdatedBy",
                table: "Species",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Species_Users_CreatedBy",
                table: "Species");

            migrationBuilder.DropForeignKey(
                name: "FK_Species_Users_UpdatedBy",
                table: "Species");

            migrationBuilder.DropIndex(
                name: "IX_Species_CreatedBy",
                table: "Species");

            migrationBuilder.DropIndex(
                name: "IX_Species_UpdatedBy",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Species");
        }
    }
}
