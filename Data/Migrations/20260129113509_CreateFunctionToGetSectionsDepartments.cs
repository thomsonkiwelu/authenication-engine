using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunctionToGetSectionsDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fun_get_sections_departments(
	                officeid uuid)
                    RETURNS jsonb
                    LANGUAGE 'plpgsql'
                AS $BODY$
                DECLARE
                    result JSONB;
                BEGIN
                    /*
                      This function loads organization context data:
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
                            WITH sections_with_row AS (
                                SELECT 
                                    ROW_NUMBER() OVER (ORDER BY s.""CreatedAt"" DESC) as row_number,
                                    s.*
                                FROM ""Sections"" s
                                WHERE s.""OfficeId"" = officeId
                                AND s.""DeletedAt"" IS NULL
                            )
                            SELECT COALESCE(
                                jsonb_agg(
                                    jsonb_build_object(
                                        'rowNumber', swr.row_number,
                                        'Id', swr.""Id"",
                                        'Name', swr.""Name"", 
                                        'Code', swr.""Code"",
                                        'OfficeId', swr.""OfficeId"",
                                        'DepartmentId', swr.""DepartmentId"",
                                        'CreatedAt', swr.""CreatedAt"", 
                                        'CreatedBy', swr.""CreatedBy"", 
                                        'UpdatedAt', swr.""UpdatedAt"",
                                        'UpdatedBy', swr.""UpdatedBy"",
						                'Department', jsonb_build_object(
							                'id', d.""Id"", 
							                'name', d.""Name"",
							                'code', d.""Code"", 
							                'officeId', d.""OfficeId"",
							                'createdAt', d.""CreatedAt"",
							                'createdBy', d.""CreatedBy""
						                )
                                    )
                                    ORDER BY swr.""CreatedAt"" DESC
                                ), 
                                '[]'::jsonb
                            )
                            FROM sections_with_row swr
                            JOIN ""Departments"" d ON d.""Id"" = swr.""DepartmentId"" AND d.""DeletedAt"" IS NULL
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
                DROP FUNCTION IF EXISTS public.fun_get_sections_departments(uuid);
            ");
        }
    }
}
