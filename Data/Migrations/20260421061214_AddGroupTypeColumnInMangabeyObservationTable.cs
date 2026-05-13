using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupTypeColumnInMangabeyObservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGroupTypeLetu",
                table: "MangabeyObservations",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGroupTypeLingine",
                table: "MangabeyObservations",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGroupTypeLetu",
                table: "MangabeyObservations");

            migrationBuilder.DropColumn(
                name: "IsGroupTypeLingine",
                table: "MangabeyObservations");
        }
    }
}
