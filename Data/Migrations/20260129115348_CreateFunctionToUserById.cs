using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunctionToUserById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fun_user_by_id(
					userid uuid)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE
				    result JSONB;
				    v_office_id uuid;
					v_staff_id uuid;
					v_department_info JSONB;
					v_model_type text;
					v_is_active bool;
				BEGIN
					-- Get User details
					SELECT 
					    u.""StaffId"",
						ds.""ModelType"",
						u.""IsActive"",
					    CASE ds.""ModelType""
					        WHEN 'department' THEN (
					            SELECT jsonb_build_object('id', d.""Id"", 'name', d.""Name"", 'officeId', d.""OfficeId"", 'officeName', o.""Name"")
					            FROM ""Departments"" d
					            LEFT JOIN ""Offices"" o ON o.""Id"" = d.""OfficeId""
					            WHERE d.""Id"" = ds.""DepartmentId""
					        )
					        WHEN 'unit' THEN (
					            SELECT jsonb_build_object('id', u.""Id"", 'name', u.""Name"", 'officeId', u.""OfficeId"", 'officeName', o.""Name"")
					            FROM ""Units"" u
					            LEFT JOIN ""Offices"" o ON o.""Id"" = u.""OfficeId""
					            WHERE u.""Id"" = ds.""DepartmentId""
					        )
					        WHEN 'section' THEN (
					            SELECT jsonb_build_object('id', sec.""Id"", 'name', sec.""Name"", 'officeId', sec.""OfficeId"", 'officeName', o.""Name"")
					            FROM ""Sections"" sec
					            LEFT JOIN ""Offices"" o ON o.""Id"" = sec.""OfficeId""
					            WHERE sec.""Id"" = ds.""DepartmentId""
					        )
					        ELSE NULL
					    END
					INTO v_staff_id, v_model_type, v_is_active, v_department_info
					FROM ""Users"" u
					LEFT JOIN ""DepartmentStaffs"" ds ON ds.""StaffId"" = u.""StaffId""
					WHERE u.""Id"" = userId;
					
					-- Initilization v_office_id value
					v_office_id := (v_department_info->>'officeId')::uuid;

					-- Data to be return
				    result := jsonb_build_object(
						-- Get user details
						'user', (
				            SELECT COALESCE(
								jsonb_build_object(
									'staffId', sf.""Id"", 
									'firstName', sf.""FirstName"", 
									'lastName', sf.""LastName"",
									'email', sf.""Email"",
									'rankName', r.""Name"",
									'departmentId', v_department_info->>'id',
									'officeName', v_department_info->>'officeName',
									'departmentName', v_department_info->>'name',
									'modelType', v_model_type,
									'isActive', CASE WHEN v_is_active THEN '1' ELSE '0' END
								), 
							'{}'::jsonb)
				            FROM ""Staffs"" sf
							JOIN ""Ranks"" r ON r.""Id"" = sf.""RankId""
				            WHERE sf.""Id"" = v_staff_id
				        ),
						-- Get all department based on office ID
				        'departments', (
				            SELECT COALESCE(jsonb_agg(jsonb_build_object('id', d.""Id"", 'name', d.""Name"")), '[]'::jsonb)
				            FROM ""Departments"" d
				            WHERE d.""OfficeId"" = v_office_id AND d.""DeletedAt"" IS NULL
				        ),
						-- Get all section based on office ID
						'sections', (
				            SELECT COALESCE(jsonb_agg(jsonb_build_object('id', s.""Id"", 'name', s.""Name"")), '[]'::jsonb)
				            FROM ""Sections"" s
				            WHERE s.""OfficeId"" = v_office_id AND s.""DeletedAt"" IS NULL
				        ),
						-- Get all unit based on office ID
						'units', (
				            SELECT COALESCE(jsonb_agg(jsonb_build_object('id', u.""Id"", 'name', u.""Name"")), '[]'::jsonb)
				            FROM ""Units"" u
				            WHERE u.""OfficeId"" = v_office_id AND u.""DeletedAt"" IS NULL
				        ),
						-- Get all role that are not assigned to user
						'roles', (
							SELECT COALESCE(jsonb_agg(json_build_object('id', r.""Id"", 'name', r.""Name"")), '[]'::jsonb)  
				            FROM ""Roles"" r
							WHERE r.""DeletedAt"" IS NULL
							AND NOT EXISTS (
				                SELECT 1 FROM ""RoleUsers"" ru
				                WHERE ru.""RoleId"" = r.""Id""
				                AND ru.""UserId"" = userId
				            )
				        ),
						-- Get all role assigned to user
						'assignedRoles', (
							WITH assigned_role_row AS (
								SELECT
									ROW_NUMBER() OVER (ORDER BY ru.""CreatedAt"" DESC) as row_number,
									ru.*,
									r.""Id"",
									r.""Name"",
									r.""Description"",
									r.""Slug"",
									u.""Username""
								FROM ""RoleUsers"" ru
								JOIN ""Roles"" r ON r.""Id"" = ru.""RoleId""
								JOIN ""Users"" u ON u.""Id"" = ru.""CreatedBy""
								WHERE ru.""UserId"" = userId
				            )
							SELECT COALESCE(jsonb_agg(
								json_build_object(
									'rowNumber', arw.row_number,
									'id', arw.""Id"",
									'name', arw.""Name"",
									'description', arw.""Description"",
									'slug', arw.""Slug"",
									'createdAt', arw.""CreatedAt"",
									'createdBy', arw.""Username""
								)
								ORDER BY arw.""CreatedAt"" DESC
							), '[]'::jsonb)
							FROM assigned_role_row arw
						),
						-- Get all the staff histories of user
				        'staffHistories', (
				            WITH staff_histories_with_row AS (
								SELECT 
									ROW_NUMBER() OVER (ORDER BY dsh.""CreatedAt"" DESC) as row_number,
									dsh.*
								FROM ""DepartmentStaffHistories"" dsh
								WHERE dsh.""StaffId"" = v_staff_id
				            )
				            SELECT COALESCE(
				                jsonb_agg(
				                    jsonb_build_object(
				                        'rowNumber', shr.row_number,
				                        'Id', shr.""Id"",
				                        'StaffId', shr.""StaffId"",
										'DepartmentId', shr.""DepartmentId"",
										'ModelType', shr.""ModelType"",
				                        'CreatedAt', shr.""CreatedAt"", 
				                        'CreatedBy', shr.""CreatedBy"", 
										'Department', 
							                CASE shr.""ModelType""
							                    WHEN 'department' THEN (
													SELECT COALESCE(jsonb_build_object('name', d.""Name"", 'officeName', o.""Name""), '{}'::jsonb)
														FROM ""Departments"" d 
														JOIN ""Offices"" o ON o.""Id"" = d.""OfficeId""
													WHERE d.""Id"" = shr.""DepartmentId""
												)
							                    WHEN 'unit' THEN (
													SELECT COALESCE(jsonb_build_object('name', u.""Name"", 'officeName', o.""Name""), '{}'::jsonb)
														FROM ""Units"" u
														JOIN ""Offices"" o ON o.""Id"" = u.""OfficeId""
													WHERE u.""Id"" = shr.""DepartmentId""
												)
							                    WHEN 'section' THEN (
													SELECT COALESCE(jsonb_build_object('name', sec.""Name"", 'officeName', o.""Name""), '{}'::jsonb)
														FROM ""Sections"" sec
														JOIN ""Offices"" o ON o.""Id"" = sec.""OfficeId""
													WHERE sec.""Id"" = shr.""DepartmentId""
												)
							                    ELSE NULL
							                END,
										'User', jsonb_build_object(
											'id', u.""Id"",
											'username', u.""Username""
										)
				                    )
									ORDER BY shr.""CreatedAt"" DESC
				                ),
				                '[]'::jsonb
				            )
				            FROM staff_histories_with_row shr
				            JOIN ""Users"" u ON u.""Id"" = shr.""CreatedBy""
				        )
				    );
				    
				    RETURN COALESCE(result, '{}'::jsonb);
				END;
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fun_user_by_id(uuid);
            ");
        }
    }
}
