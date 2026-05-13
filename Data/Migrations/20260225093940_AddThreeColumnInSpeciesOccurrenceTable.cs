using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddThreeColumnInSpeciesOccurrenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "EstimatedNumber",
                table: "SpeciesOccurrences",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ObservedNumber",
                table: "SpeciesOccurrences",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StandardError",
                table: "SpeciesOccurrences",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedNumber",
                table: "SpeciesOccurrences");

            migrationBuilder.DropColumn(
                name: "ObservedNumber",
                table: "SpeciesOccurrences");

            migrationBuilder.DropColumn(
                name: "StandardError",
                table: "SpeciesOccurrences");
        }
    }
}
