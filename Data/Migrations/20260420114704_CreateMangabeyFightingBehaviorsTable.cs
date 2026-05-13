using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMangabeyFightingBehaviorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MangabeyFightingBehaviors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AggressiveIndividual = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AttackedIndividual = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WhatHappened = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MangabeyObservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangabeyFightingBehaviors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangabeyFightingBehaviors_MangabeyObservations_MangabeyObse~",
                        column: x => x.MangabeyObservationId,
                        principalTable: "MangabeyObservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangabeyFightingBehaviors_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MangabeyFightingBehaviors_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyFightingBehaviors_CreatedBy",
                table: "MangabeyFightingBehaviors",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyFightingBehaviors_MangabeyObservationId",
                table: "MangabeyFightingBehaviors",
                column: "MangabeyObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_MangabeyFightingBehaviors_UpdatedBy",
                table: "MangabeyFightingBehaviors",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangabeyFightingBehaviors");
        }
    }
}
