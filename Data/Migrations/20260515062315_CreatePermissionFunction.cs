using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication_engine.Migrations
{
    /// <inheritdoc />
    public partial class CreatePermissionFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_create_permission(
					input_data jsonb
				)
				    RETURNS boolean
				    LANGUAGE 'plpgsql'
				AS $BODY$
				    DECLARE
						v_permission_element JSONB;
						v_system_application_id uuid;
					BEGIN
						-- Get System Application Id
						SELECT ""SystemApplicationId"" INTO v_system_application_id 
						FROM ""SystemModules""
						WHERE ""Id"" = (input_data->>'SystemModuleId')::UUID;
						
						-- Insert Permissions Data
						IF input_data->'Permissions' IS NOT NULL AND jsonb_array_length(input_data->'Permissions') > 0 THEN
							FOR v_permission_element IN
								SELECT * FROM jsonb_array_elements(input_data->'Permissions')
							LOOP
								BEGIN
									INSERT INTO public.""Permissions""(
										""Id"",
										""Name"",
										""Action"",
										""ModelType"",
										""SystemModuleId"",
										""SystemApplicationId"",
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_permission_element->>'Name',
										v_permission_element->>'Action',
										input_data->>'ModelType',
										(input_data->>'SystemModuleId')::UUID,
										v_system_application_id,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'Permissions table error: %', SQLERRM;
								END;
							END LOOP;
						END IF;
						
						-- Return success
						RETURN true;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_permission: %', SQLERRM;
					END;			
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_create_permission(jsonb);
            ");
        }
    }
}
