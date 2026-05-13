using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunctionToUnassignStaffFromDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fun_unassign_staff_from_department(
					input_data jsonb)
				    RETURNS boolean
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE
				    v_staff_id uuid;
					v_office_id uuid;
					v_current_user uuid;
					v_user_id uuid;
				BEGIN
				    -- Extract values from JSON input
				    v_staff_id := (input_data->>'StaffId')::uuid;
					v_office_id := (input_data->>'OfficeId')::uuid;
				    v_current_user := (input_data->>'CurrentUser')::uuid;

					-- Check if user exists and get ID
				    SELECT ""Id"" INTO v_user_id FROM ""Users"" WHERE ""StaffId"" = v_staff_id;

					-- Store staff history
				    INSERT INTO ""DepartmentStaffHistories"" (""Id"", ""StaffId"", ""DepartmentId"", ""ModelType"", ""CreatedBy"", ""CreatedAt"")
				    SELECT 
				        gen_random_uuid(),
				        s.""StaffId"",
						s.""DepartmentId"",
						s.""ModelType"",
				        v_current_user,
				        NOW()
				    FROM ""DepartmentStaffs"" s
				    WHERE s.""StaffId"" = v_staff_id;
					
					-- Remove data from DepartmentStaffs
					DELETE FROM ""DepartmentStaffs"" WHERE ""StaffId"" = v_staff_id;

					-- Remove data from RoleUser
					DELETE FROM ""RoleUsers"" WHERE ""UserId"" = v_user_id;

					-- Remove data from UserParks
					DELETE FROM ""UserParks"" WHERE ""UserId"" = v_user_id;
					
					-- Update users tables
					UPDATE ""Users"" SET
						""IsActive"" = false,
						""UpdatedBy"" = v_current_user,
						""UpdatedAt"" = NOW()
					WHERE ""Id"" = v_user_id;
				    
				    -- Return success
				    RETURN true;
				    
				EXCEPTION
				    WHEN OTHERS THEN
						RAISE EXCEPTION 'Error in unassign_staff_from_department: %', SQLERRM;
				        RETURN false;
				END;
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fun_unassign_staff_from_department(jsonb);
            ");
        }
    }
}
