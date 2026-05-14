using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication_engine.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunctionToGetUnitsDepartmentsAndSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fun_get_units_departments_sections(
	                officeid uuid)
                    RETURNS jsonb
                    LANGUAGE 'plpgsql'
                AS $BODY$
                DECLARE 
                    result JSONB;
                BEGIN
                    /*
                      This function loads organization context data:
                      - Units
                      - Departments
                      - Sections
                      Filtered by officeId where applicable
                    */
                    SELECT jsonb_build_object(
                        'departments', (
                            SELECT COALESCE(jsonb_agg(jsonb_build_object('id', d.""Id"", 'name', d.""Name"")), '[]'::jsonb)
                            FROM ""Departments"" d
                            WHERE d.""OfficeId"" = officeId AND d.""DeletedAt"" IS NULL
                        ),
                        'sections', (
                            SELECT COALESCE(jsonb_agg(jsonb_build_object('id', s.""Id"", 'name', s.""Name"")), '[]'::jsonb) 
                            FROM ""Sections"" s
                            WHERE s.""OfficeId"" = officeId AND s.""DeletedAt"" IS NULL
                        ),
                        'units', (
                            WITH units_with_row AS (
                                SELECT 
                                    ROW_NUMBER() OVER (ORDER BY u.""CreatedAt"" DESC) as row_number,
                                    u.*
                                FROM ""Units"" u
                                WHERE u.""OfficeId"" = officeId
                                AND u.""DeletedAt"" IS NULL
                            )
                            SELECT COALESCE(
                                jsonb_agg(
                                    jsonb_build_object(
                                        'rowNumber', uwr.row_number,
                                        'Id', uwr.""Id"",
                                        'Name', uwr.""Name"", 
                                        'Code', uwr.""Code"",
                                        'OfficeId', uwr.""OfficeId"",
                                        'DepartmentId', uwr.""DepartmentId"",
                                        'SectionId', uwr.""SectionId"",
                                        'CreatedAt', uwr.""CreatedAt"", 
                                        'CreatedBy', uwr.""CreatedBy"", 
                                        'UpdatedAt', uwr.""UpdatedAt"",
                                        'UpdatedBy', uwr.""UpdatedBy"",
						                'Department',
	                                        CASE 
	                                            WHEN d.""Id"" IS NULL THEN 
	                                                'null'::jsonb
	                                            ELSE
	                                                jsonb_build_object(
	                                                    'id', d.""Id"", 
	                                                    'name', d.""Name"",
	                                                    'code', d.""Code"", 
	                                                    'officeId', d.""OfficeId"",
	                                                    'createdAt', d.""CreatedAt"",
	                                                    'createdBy', d.""CreatedBy""
	                                                )
	                                        END,
                                        'Section', 
	                                        CASE 
	                                            WHEN s.""Id"" IS NULL THEN 
	                                                'null'::jsonb
	                                            ELSE
	                                                jsonb_build_object(
	                                                    'id', s.""Id"", 
	                                                    'name', s.""Name"",
	                                                    'code', s.""Code"", 
	                                                    'officeId', s.""OfficeId"",
	                                                    'createdAt', s.""CreatedAt"",
	                                                    'createdBy', s.""CreatedBy""
	                                                )
	                                        END
                                    )
                                    ORDER BY uwr.""CreatedAt"" DESC
                                ), 
                                '[]'::jsonb
                            )
                            FROM units_with_row uwr
                            LEFT JOIN ""Departments"" d ON d.""Id"" = uwr.""DepartmentId"" AND d.""DeletedAt"" IS NULL
			                LEFT JOIN ""Sections"" s ON s.""Id"" = uwr.""SectionId"" AND s.""DeletedAt"" IS NULL
                        )
                    ) INTO result;
                    
                    RETURN COALESCE(result, '{}'::jsonb);
                END;
                $BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS public.fun_get_units_departments_sections(uuid);
            ");
        }
    }
}
