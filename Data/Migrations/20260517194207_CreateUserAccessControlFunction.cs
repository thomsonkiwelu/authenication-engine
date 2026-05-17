using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication_engine.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserAccessControlFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fun_get_user_access_data(
					user_id uuid, 
					system_application_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						user_data JSONB;
					BEGIN
						-- Get user with roles, permissions, and modules
						SELECT jsonb_build_object(
							'Id', u.""Id"",
							'Username', u.""Username"",
							'Email', u.""Email"",
							'IsActive', u.""IsActive"",
							'Staff', jsonb_build_object(
								'Id', st.""Id"",
								'TnpNumber', st.""TnpNumber"",
								'FirstName', st.""FirstName"", 
								'MiddleName', st.""MiddleName"",
								'LastName', st.""LastName"",
								'Email', st.""Email"",
								'PhoneNumber', st.""PhoneNumber"",
								'Status', st.""Status"",
								'RankId', st.""RankId"",
								'CreatedAt', st.""CreatedAt"",
								'CreatedBy', st.""CreatedBy"",
								'Rank', (
									SELECT jsonb_build_object(
										'Id', r.""Id"",
										'Name', r.""Name"",
										'Code', r.""Code"",
										'Level', r.""Level"",
										'CreatedAt', r.""CreatedAt"",
										'CreatedBy', r.""CreatedBy""
									)
									FROM ""Ranks"" r
									WHERE r.""Id"" = st.""RankId""
								)
							),
							-- SystemApplication
							'SystemApplication',(
								SELECT jsonb_build_object(
									'Id', sa.""Id"",
									'Name', sa.""Name"",
									'Slug', sa.""Slug"",
									'CreatedAt', sa.""CreatedAt"",
									'CreatedBy', sa.""CreatedBy""
								)
								FROM ""SystemApplications"" sa
								WHERE sa.""Id"" = system_application_id
								AND sa.""DeletedAt"" IS NULL
							),
							-- Modules based SystemApplicationId
							'Modules', COALESCE((
							    SELECT jsonb_agg(
							        DISTINCT jsonb_build_object(
							            'Id', sm.""Id"", 
							            'Name', sm.""Name"", 
							            'Slug', sm.""Slug""
							        )
							    )
							    FROM ""RoleUsers"" ru
							    JOIN ""Roles"" r ON ru.""RoleId"" = r.""Id""
							    JOIN ""SystemModules"" sm ON sm.""SystemApplicationId"" = r.""SystemApplicationId""
							    WHERE ru.""UserId"" = u.""Id""
							    AND r.""SystemApplicationId"" = system_application_id
							), '[]'::jsonb),
							-- Roles based SystemApplicationId
							'Roles', COALESCE((SELECT jsonb_agg(
								jsonb_build_object(
									'Id', r.""Id"", 
									'Name', r.""Name"", 
									'Slug', r.""Slug"", 
									'Description', r.""Description"", 
									'CreatedAt', r.""CreatedAt""
								))
								FROM ""RoleUsers"" ru
								JOIN ""Roles"" r ON ru.""RoleId"" = r.""Id""
								WHERE ru.""UserId"" = u.""Id"" 
								AND r.""SystemApplicationId"" = system_application_id
								AND r.""DeletedAt"" IS NULL), '[]'::jsonb
							),
							-- Permissions based SystemApplicationId
							'Permissions', COALESCE((SELECT jsonb_agg
								(jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Action', p.""Action"",
									'ModelType', p.""ModelType""
								))
								FROM ""RoleUsers"" ru
								JOIN ""RolePermissions"" rp ON ru.""RoleId"" = rp.""RoleId""
								JOIN ""Permissions"" p ON rp.""PermissionId"" = p.""Id""
								WHERE ru.""UserId"" = u.""Id""
								AND p.""SystemApplicationId"" = system_application_id
								AND p.""DeletedAt"" IS NULL), '[]'::jsonb
							),
							-- Office Details
							'Office',(
								SELECT jsonb_build_object(
									'Id', o.""Id"",
									'Name', o.""Name"",
									'Code', o.""Code"",
									'ParentOffice', o.""ParentOffice"",
									'HeadOfOffice', o.""HeadOfOffice"",
									'StructureId', o.""StructureId"",
									'ParkId', o.""ParkId"",
									'CreatedAt', o.""CreatedAt"",
									'CreatedBy', o.""CreatedBy""
								)
								FROM ""Offices"" o
								WHERE o.""Id"" = ds.""OfficeId""
								AND o.""DeletedAt"" IS NULL
							),
							-- AssignedPark based on Office ParkId
					        'AssignedPark', (
							    SELECT jsonb_build_object(
							        'Id', p.""Id"",
							        'Name', p.""Name"",
							        'Code', COALESCE(p.""Code"", ''),
							        'Zone', p.""Zone""
							    )
							    FROM ""Offices"" o
							    LEFT JOIN ""Parks"" p ON p.""Id"" = o.""ParkId""
							    WHERE o.""Id"" = ds.""OfficeId""
							    AND o.""DeletedAt"" IS NULL
							    AND o.""ParkId"" IS NOT NULL
							),
							-- AssignedParks
							'AccessibleParks', COALESCE((SELECT jsonb_agg
								(jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone""
								))
								FROM ""UserParks"" up
								JOIN ""Parks"" p ON p.""Id"" = up.""ParkId""
								WHERE up.""UserId"" = u.""Id""), '[]'::jsonb
							)
						) INTO user_data
						FROM ""Users"" u
						JOIN ""Staffs"" st ON u.""StaffId"" = st.""Id""
						JOIN ""DepartmentStaffs"" ds ON ds.""StaffId"" = u.""StaffId""
						WHERE u.""Id"" = user_id;
						
						-- data to be return
						RETURN COALESCE(user_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fun_get_user_access_data: %', SQLERRM;
					END;		
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fun_get_user_access_data(uuid, uuid);
            ");
        }
    }
}
