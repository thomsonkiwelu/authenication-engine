using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLessHwcConfigAndIncidents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessHwcIncidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParkId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfficeId = table.Column<Guid>(type: "uuid", nullable: true),
                    IncidentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    District = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Ward = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Villages = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IncidentCategoryKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ReferenceNo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TotalIncidents = table.Column<int>(type: "integer", nullable: false),
                    EstimatedLossTzs = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    DataJson = table.Column<string>(type: "jsonb", maxLength: 50000, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessHwcIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessHwcIncidents_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LessHwcIncidents_Parks_ParkId",
                        column: x => x.ParkId,
                        principalTable: "Parks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessHwcIncidents_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessHwcIncidents_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessHwcTabDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessHwcTabDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessHwcTabDefinitions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessHwcTabDefinitions_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessHwcFieldDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TabDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FieldType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Placeholder = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IsComputed = table.Column<bool>(type: "boolean", nullable: false),
                    ComputeFromKeys = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessHwcFieldDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessHwcFieldDefinitions_LessHwcTabDefinitions_TabDefinition~",
                        column: x => x.TabDefinitionId,
                        principalTable: "LessHwcTabDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessHwcFieldDefinitions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessHwcFieldDefinitions_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessHwcFieldOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FieldDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessHwcFieldOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessHwcFieldOptions_LessHwcFieldDefinitions_FieldDefinition~",
                        column: x => x.FieldDefinitionId,
                        principalTable: "LessHwcFieldDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessHwcFieldOptions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessHwcFieldOptions_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcFieldDefinitions_CreatedBy",
                table: "LessHwcFieldDefinitions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcFieldDefinitions_TabDefinitionId_Key",
                table: "LessHwcFieldDefinitions",
                columns: new[] { "TabDefinitionId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcFieldDefinitions_UpdatedBy",
                table: "LessHwcFieldDefinitions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcFieldOptions_CreatedBy",
                table: "LessHwcFieldOptions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcFieldOptions_FieldDefinitionId_Value",
                table: "LessHwcFieldOptions",
                columns: new[] { "FieldDefinitionId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcFieldOptions_UpdatedBy",
                table: "LessHwcFieldOptions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcIncidents_CreatedBy",
                table: "LessHwcIncidents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcIncidents_OfficeId_IncidentDate",
                table: "LessHwcIncidents",
                columns: new[] { "OfficeId", "IncidentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcIncidents_ParkId_IncidentDate",
                table: "LessHwcIncidents",
                columns: new[] { "ParkId", "IncidentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcIncidents_UpdatedBy",
                table: "LessHwcIncidents",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcTabDefinitions_CreatedBy",
                table: "LessHwcTabDefinitions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcTabDefinitions_Key",
                table: "LessHwcTabDefinitions",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessHwcTabDefinitions_UpdatedBy",
                table: "LessHwcTabDefinitions",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessHwcFieldOptions");

            migrationBuilder.DropTable(
                name: "LessHwcIncidents");

            migrationBuilder.DropTable(
                name: "LessHwcFieldDefinitions");

            migrationBuilder.DropTable(
                name: "LessHwcTabDefinitions");
        }
    }
}
