using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateVillageContextFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_village_context(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
	 					v_entity_name CONSTANT VARCHAR(50) := 'VillageContexts';
						v_village_context_id UUID;
						v_array_element TEXT;
					BEGIN
						-- Insert VillageContexts data
						BEGIN
							INSERT INTO public.""VillageContexts""(
								""Id"",
								""Name"",
								""Data"",
								""Description"",
								""EntityId"", 
								""EntityName"", 
								""FieldName"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								input_data->>'Name',
								input_data->>'Data',
								NULLIF(COALESCE(input_data->>'Description', ''),''),
								(input_data->>'EntityId')::UUID,
								input_data->>'EntityName',
								input_data->>'FieldName',
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_village_context_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'VillageContexts table error: %', SQLERRM;
						END;
						
						-- Insert Identifications Data
						IF input_data->'Identifications' IS NOT NULL AND jsonb_array_length(input_data->'Identifications') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'Identifications')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"",
										""Value"",
										""FieldName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'NonGovernmentOrganization',
										v_village_context_id,
										input_data->>'EntityName',
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (Identifications) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return the created v_village_context_id
						RETURN v_village_context_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_village_context: %', SQLERRM;
					END;					
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_village_context(
					input_data jsonb,
					village_context_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_village_context_id UUID;
						v_array_element TEXT;
					BEGIN
						-- Convert village_context_id parameter to UUID
						v_village_context_id := village_context_id::UUID;
						
						-- Update VillageContexts data
						BEGIN
							UPDATE public.""VillageContexts""
							SET
								""Name"" = input_data->>'Name',
								""Data"" = input_data->>'Data',
								""Description"" = NULLIF(COALESCE(input_data->>'Description', ''),''),
								""EntityId"" = (input_data->>'EntityId')::UUID,
								""EntityName"" = input_data->>'EntityName',
								""FieldName"" = input_data->>'FieldName',
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_village_context_id
							RETURNING ""Id"" INTO v_village_context_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'VillageContexts table error: %', SQLERRM;
						END;
						
						-- Insert Identifications Data
						IF input_data->'Identifications' IS NOT NULL AND jsonb_array_length(input_data->'Identifications') > 0 THEN
						
							-- Delete existing Identifications
							DELETE FROM public.""CommunitySelections""
							WHERE ""EntityId"" = v_village_context_id
							AND ""FieldName"" = 'NonGovernmentOrganization';
											
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'Identifications')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"",
										""Value"",
										""FieldName"",
										""EntityId"",
										""EntityName"",
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'NonGovernmentOrganization',
										v_village_context_id,
										input_data->>'EntityName',
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (Identifications) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return the created v_village_context_id
						RETURN v_village_context_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_village_context: %', SQLERRM;
					END;					
				$BODY$;
            ");

            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_village_context_by_id(
					village_context_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', vc.""Id"",
								'Name', vc.""Name"",
								'Data', vc.""Data"",
								'Description', vc.""Description"",
								'EntityId', vc.""EntityId"",
								'EntityName', vc.""EntityName"",
								'FieldName', vc.""FieldName"",
								'CreatedBy', vc.""CreatedBy"",
								'CreatedAt', vc.""CreatedAt"",
								'UpdatedBy', vc.""UpdatedBy"",
								'UpdatedAt', vc.""UpdatedAt"",
								'Identifications', (
						            SELECT COALESCE(jsonb_agg(
						                jsonb_build_object(
						                    'Id', cs.""Id"",
						                    'Value', cs.""Value"",
						                    'OtherName', cs.""OtherName"",
											'EntityId', cs.""EntityId"",
						                    'EntityName', cs.""EntityName"",
						                    'CreatedAt', cs.""CreatedAt"",
						                    'CreatedBy', cs.""CreatedBy""
						                )
						            ), '[]'::jsonb)
						            FROM ""CommunitySelections"" cs
						            WHERE cs.""EntityId"" = vc.""Id""
						            AND cs.""FieldName"" = 'NonGovernmentOrganization'
						        ),
								'Creator', jsonb_build_object(
									'Id', u.""Id"",
									'Username', u.""Username"",
									'Email', u.""Email""
								),
								'Updater', (
									SELECT jsonb_build_object(
										'Id', u.""Id"",
										'Username', u.""Username"",
										'Email', u.""Email""
									)
									FROM ""Users"" u
									WHERE u.""Id"" = vc.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""VillageContexts"" vc
						JOIN ""Users"" u ON u.""Id"" = vc.""CreatedBy""
						WHERE vc.""Id"" = village_context_id;
						
						-- data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_village_context_by_id: %', SQLERRM;
					END;	
				$BODY$;
			");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_create_village_context(jsonb);
				DROP FUNCTION IF EXISTS public.fn_update_village_context(jsonb, character varying);
				DROP FUNCTION IF EXISTS public.fn_village_context_by_id(uuid);
            ");
        }
    }
}
