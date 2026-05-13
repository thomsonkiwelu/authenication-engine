using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLevelColumnInRankTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Ranks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Ranks");
        }
    }
}
