using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateLifeFormsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vegetations_Species_SpeciesId",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "CanopyClosure",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "CanopyDiameter",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "Circumference",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "Cover",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "Diameter",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "DiameterAtBreastHeight",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "FamilyName",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Vegetations");

            migrationBuilder.DropColumn(
                name: "LifeForm",
                table: "Vegetations");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Vegetations",
                newName: "SpeciesCount");

            migrationBuilder.RenameColumn(
                name: "StemNumber",
                table: "Vegetations",
                newName: "CommonNumber");

            migrationBuilder.AlterColumn<Guid>(
                name: "SpeciesId",
                table: "Vegetations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "OtherMethodology",
                table: "Vegetations",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LifeForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FamilyName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uuid", nullable: false),
                    Height = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Weight = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StemNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Diameter = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Cover = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    DiameterAtBreastHeight = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Circumference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CanopyDiameter = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CanopyClosure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VegetationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LifeForms_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LifeForms_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LifeForms_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LifeForms_Vegetations_VegetationId",
                        column: x => x.VegetationId,
                        principalTable: "Vegetations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LifeForms_CreatedBy",
                table: "LifeForms",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LifeForms_SpeciesId",
                table: "LifeForms",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_LifeForms_UpdatedBy",
                table: "LifeForms",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LifeForms_VegetationId",
                table: "LifeForms",
                column: "VegetationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vegetations_Species_SpeciesId",
                table: "Vegetations",
                column: "SpeciesId",
                principalTable: "Species",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vegetations_Species_SpeciesId",
                table: "Vegetations");

            migrationBuilder.DropTable(
                name: "LifeForms");

            migrationBuilder.DropColumn(
                name: "OtherMethodology",
                table: "Vegetations");

            migrationBuilder.RenameColumn(
                name: "SpeciesCount",
                table: "Vegetations",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "CommonNumber",
                table: "Vegetations",
                newName: "StemNumber");

            migrationBuilder.AlterColumn<Guid>(
                name: "SpeciesId",
                table: "Vegetations",
                type: "uuid",
                maxLength: 150,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CanopyClosure",
                table: "Vegetations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CanopyDiameter",
                table: "Vegetations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Circumference",
                table: "Vegetations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "Vegetations",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diameter",
                table: "Vegetations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiameterAtBreastHeight",
                table: "Vegetations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                table: "Vegetations",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Vegetations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifeForm",
                table: "Vegetations",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Vegetations_Species_SpeciesId",
                table: "Vegetations",
                column: "SpeciesId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
