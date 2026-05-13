using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddFunctionToGetUserByIdWithAccessControlList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fun_get_user_by_id_with_access_control(
					user_id uuid
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
								'FirstName', st.""FirstName"", 
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
							'Modules', COALESCE((SELECT jsonb_agg(
								DISTINCT jsonb_build_object(
									'Id', sm.""Id"", 
									'Name', sm.""Name"", 
									'Slug', sm.""Slug""
			 					))
			 					FROM ""RoleUsers"" ru
			 					JOIN ""RolePermissions"" rp ON ru.""RoleId"" = rp.""RoleId""
			 					JOIN ""Permissions"" p ON rp.""PermissionId"" = p.""Id""
			 					JOIN ""SystemModules"" sm ON p.""SystemModuleId"" = sm.""Id""
			 					WHERE ru.""UserId"" = u.""Id""), '[]'::jsonb
							),
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
			 					WHERE ru.""UserId"" = u.""Id""), '[]'::jsonb
							),
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
		 						WHERE ru.""UserId"" = u.""Id""), '[]'::jsonb
							),
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
							RAISE EXCEPTION 'Error in fun_get_user_by_id_with_access_control: %', SQLERRM;
					END;          
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS FUN_GET_USER_BY_ID_WITH_ACCESS_CONTROL(user_id UUID);");
        }
    }
}
