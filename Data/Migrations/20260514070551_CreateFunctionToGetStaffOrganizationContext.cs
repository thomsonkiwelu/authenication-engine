using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication_engine.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunctionToGetStaffOrganizationContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fun_get_staffs_organization_context(
					officeid uuid)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE 
				    result JSONB;
				BEGIN
					/*
				      This function loads organization context data:
				      - Staffs
				      - Units
				      - Departments
				      - Sections
				      - Roles
					  - AssignedStaffs
				      Filtered by officeId where applicable
				    */
				    SELECT jsonb_build_object(
						-- add logic is staff is already assigned should not appear in below list
						'staffs', (
				            SELECT COALESCE(array_agg(json_build_object('id', sf.""Id"", 'name', CONCAT(sf.""FirstName"", ' ', sf.""LastName""))), ARRAY[]::json[])  
				            FROM ""Staffs"" sf
							WHERE sf.""DeletedAt"" IS NULL
							AND NOT EXISTS (
				                SELECT 1 FROM ""DepartmentStaffs"" ds 
				                WHERE ds.""StaffId"" = sf.""Id""
				                AND ds.""DeletedAt"" IS NULL
				            )
				        ),
				        'units', (
				            SELECT COALESCE(jsonb_agg(jsonb_build_object('id', u.""Id"", 'name', u.""Name"")), '[]'::jsonb)  
				            FROM ""Units"" u 
				            WHERE u.""OfficeId"" = officeId AND u.""DeletedAt"" IS NULL
				        ),
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
						'roles', (
				            SELECT COALESCE(jsonb_agg(jsonb_build_object('id', r.""Id"", 'name', r.""Name"")), '[]'::jsonb) 
				            FROM ""Roles"" r
							WHERE r.""DeletedAt"" IS NULL
				        ),
						'assignedStaffs', (
						    WITH numbered_staffs AS (
						        SELECT 
						            ds.*,
						            s.""Id"" as staff_id,
						            s.""FirstName"",
						            s.""LastName"", 
						            s.""Email"",
						            s.""PhoneNumber"",
						            ROW_NUMBER() OVER (ORDER BY ds.""CreatedAt"" DESC) as row_number
						        FROM ""DepartmentStaffs"" ds
						        JOIN ""Staffs"" s ON s.""Id"" = ds.""StaffId"" 
						        WHERE ds.""OfficeId"" = officeId 
						        AND ds.""DeletedAt"" IS NULL
						    )
						    SELECT COALESCE(jsonb_agg(
						        jsonb_build_object(
						            'rowNumber', ns.row_number,
						            'Id', ns.staff_id,
						            'FirstName', ns.""FirstName"", 
						            'LastName', ns.""LastName"", 
						            'Email', ns.""Email"", 
						            'PhoneNumber', ns.""PhoneNumber"", 
						            'ModelType', ns.""ModelType"",
						            'AssignedTo', 
						                CASE ns.""ModelType""
						                    WHEN 'department' THEN (SELECT d.""Name"" FROM ""Departments"" d WHERE d.""Id"" = ns.""DepartmentId"")
						                    WHEN 'unit' THEN (SELECT u.""Name"" FROM ""Units"" u WHERE u.""Id"" = ns.""DepartmentId"")
						                    WHEN 'section' THEN (SELECT sec.""Name"" FROM ""Sections"" sec WHERE sec.""Id"" = ns.""DepartmentId"")
						                    ELSE NULL
						                END,
						            'CreatedAt', ns.""CreatedAt""
						        )
						    ), '[]'::jsonb)
						    FROM numbered_staffs ns
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
				DROP FUNCTION IF EXISTS public.fun_get_staffs_organization_context(uuid);
            ");
        }
    }
}
