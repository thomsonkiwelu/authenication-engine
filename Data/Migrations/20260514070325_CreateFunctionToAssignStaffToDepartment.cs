using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication_engine.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunctionToAssignStaffToDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fun_assign_staff_to_department(
					input_data jsonb
				)
				    RETURNS boolean
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_staff_id uuid;
						v_department_id uuid;
						v_office_id uuid;
						v_model_type text;
						v_role_id uuid;
						v_password text;
						v_created_by uuid;
						v_user_id uuid;
						
						-- Variables for office details
						v_office_code text;
						v_parent_office int;
						v_office_park_id uuid;
					BEGIN
						-- Extract values from JSON input
						v_staff_id := (input_data->>'StaffId')::uuid;
						v_department_id := (input_data->>'DepartmentId')::uuid;
						v_office_id := (input_data->>'OfficeId')::uuid;
						v_model_type := input_data->>'ModelType';
						v_role_id := (input_data->>'RoleId')::uuid;
						v_password := input_data->>'Password';
						v_created_by := (input_data->>'CreatedBy')::uuid;
						
						-- Validate required fields
						IF v_password IS NULL OR v_created_by IS NULL THEN
							RAISE EXCEPTION 'Password and Created_by is required';
						END IF;

						-- Check if user already exists for this staff
						SELECT ""Id"" INTO v_user_id FROM ""Users"" WHERE ""StaffId"" = v_staff_id;

						IF v_user_id IS NOT NULL THEN   
							-- UPDATE exists user
							UPDATE ""Users"" 
							SET 
								""IsActive"" = true,
								""Password"" = v_password,
								""UpdatedBy"" = v_created_by,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_user_id;
						ELSE
							-- Create user from staff and get the generated user ID
							INSERT INTO ""Users"" (""Id"", ""Username"", ""Email"", ""Password"", ""IsActive"", ""StaffId"", ""CreatedBy"", ""CreatedAt"")
							SELECT 
								gen_random_uuid(),
								LOWER(REPLACE(s.""FirstName"" || '.' || s.""LastName"", ' ', '')),
								LOWER(s.""FirstName"" || '.' || s.""LastName"" || '@tanzaniaparks.co.tz'),
								v_password,
								true,
								s.""Id"",
								v_created_by,
								NOW()
							FROM ""Staffs"" s
							WHERE s.""Id"" = v_staff_id
							RETURNING ""Id"" INTO v_user_id;
						 END IF;
						 
						-- Insert into DepartmentStaff table
						INSERT INTO ""DepartmentStaffs"" (""StaffId"", ""DepartmentId"", ""OfficeId"", ""ModelType"", ""CreatedBy"", ""CreatedAt""
						) VALUES (
							v_staff_id,
							v_department_id,
							v_office_id,
							v_model_type,
							v_created_by,
							NOW()
						);

						 -- Insert into RoleUser table
						INSERT INTO ""RoleUsers"" (""RoleId"", ""UserId"", ""CreatedBy"", ""CreatedAt""
						) VALUES (
							v_role_id,
							v_user_id,
							v_created_by,
							NOW()
						);

						-- Get office details
						SELECT ""Code"", ""ParentOffice"", ""ParkId"" 
						INTO v_office_code, v_parent_office, v_office_park_id
						FROM ""Offices"" WHERE ""Id"" = v_office_id;

						-- Insert UserParks
						IF  v_parent_office = 1 AND v_office_code = 'EZ' THEN
							-- Insert Eastern zone parks
							INSERT INTO ""UserParks"" (""UserId"", ""ParkId"", ""CreatedBy"", ""CreatedAt"")
							SELECT 
								v_user_id, 
								p.""Id"", 
								v_created_by, 
								NOW()
							FROM ""Parks"" p
							WHERE ""Zone"" = '1';
						ELSIF v_parent_office = 1 AND v_office_code = 'WZ' THEN
							-- Insert Weastern zone parks
							INSERT INTO ""UserParks"" (""UserId"", ""ParkId"", ""CreatedBy"", ""CreatedAt"")
							SELECT 
								v_user_id, 
								p.""Id"", 
								v_created_by, 
								NOW()
							FROM ""Parks"" p
							WHERE ""Zone"" = '2';
						ELSIF v_parent_office = 1 AND v_office_code = 'NZ' THEN
							-- Insert Northern zone parks
							INSERT INTO ""UserParks"" (""UserId"", ""ParkId"", ""CreatedBy"", ""CreatedAt"")
							SELECT 
								v_user_id, 
								p.""Id"", 
								v_created_by, 
								NOW()
							FROM ""Parks"" p
							WHERE ""Zone"" = '3';
						ELSIF v_parent_office = 1 AND v_office_code = 'SZ' THEN
							-- Insert Southern zone parks
							INSERT INTO ""UserParks"" (""UserId"", ""ParkId"", ""CreatedBy"", ""CreatedAt"")
							SELECT 
								v_user_id,
								p.""Id"", 
								v_created_by, 
								NOW()
							FROM ""Parks"" p
							WHERE ""Zone"" = '4';
						ELSE
							-- Insert parks by using specificed v_office_park_id in offices table
							INSERT INTO ""UserParks"" (""UserId"", ""ParkId"", ""CreatedBy"", ""CreatedAt"")
							SELECT 
								v_user_id, 
								p.""Id"", 
								v_created_by, 
								NOW()
							FROM ""Parks"" p
							WHERE ""Id"" = v_office_park_id;
						END IF;
						
						-- Return success
						RETURN true;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in assign_staff_to_department: %', SQLERRM;
							RETURN false;
					END;		
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS public.fun_assign_staff_to_department(jsonb)");
        }
    }
}
