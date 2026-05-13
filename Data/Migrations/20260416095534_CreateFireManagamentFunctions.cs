using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateFireManagamentFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_fire_prescription(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_fire_prescription_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'FirePrescription';
						v_array_element TEXT;
						v_park_id UUID;
					BEGIN  
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Insert FirePrescriptions data
						BEGIN
							INSERT INTO public.""FirePrescriptions""(
								""Id"", 
								""LocalAreaNameId"", 
								""ParkId"",
								""ParkArea"", 
								""PlannedArea"", 
								""ActualBurntArea"",
								""Coordinates"",
								""Remark"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'LocalAreaNameId')::UUID,
								v_park_id,
								(input_data->>'ParkArea')::NUMERIC,
								(input_data->>'PlannedArea')::NUMERIC,
								(input_data->>'ActualBurntArea')::NUMERIC,
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								NULLIF(COALESCE(input_data->>'Remark', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_fire_prescription_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'FirePrescriptions table error: %', SQLERRM;
						END;

						-- Insert FireObjectives Data
						IF input_data->'FireObjectives' IS NOT NULL AND jsonb_array_length(input_data->'FireObjectives') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'FireObjectives')
							LOOP
								BEGIN
									INSERT INTO public.""EcologySelections""(
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
										'FireObjectives',
										v_fire_prescription_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (FireObjectives) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return the created v_fire_prescription_id
						RETURN v_fire_prescription_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_fire_prescription: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_fire_prescription_by_id(
					fire_prescription_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_entity_name CONSTANT VARCHAR(50) := 'FirePrescription';
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', fp.""Id"",
								'LocalAreaNameId', fp.""LocalAreaNameId"",
								'ParkId', fp.""ParkId"",
								'ParkArea', fp.""ParkArea"",
								'PlannedArea', fp.""PlannedArea"",
								'ActualBurntArea', fp.""ActualBurntArea"",
								'Coordinates', fp.""Coordinates"",
								'Remark', fp.""Remark"",
								'CreatedBy', fp.""CreatedBy"",
								'CreatedAt', fp.""CreatedAt"",
								'UpdatedBy', fp.""UpdatedBy"",
								'UpdatedAt', fp.""UpdatedAt"",
								'Location', jsonb_build_object(
									'Id', loc.""Id"",
									'Name', loc.""Name"",
									'ParkId', loc.""ParkId"",
									'CreatedBy', loc.""CreatedBy"",
									'CreatedAt', loc.""CreatedAt"",
									'Park', (
										SELECT jsonb_build_object(
											'Id', p.""Id"",
											'Name', p.""Name"",
											'Code', p.""Code"",
											'Zone', p.""Zone"",
											'CreatedAt', p.""CreatedAt"",
											'CreatedBy', p.""CreatedBy""
										)
										FROM ""Parks"" p
										WHERE p.""Id"" = loc.""ParkId""
									)
								),
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'FireObjectives', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', ad.""Id"",
											'Value', ad.""Value"",
											'FieldName', ad.""FieldName"",
											'EntityId', ad.""EntityId"",
											'EntityName', ad.""EntityName"",
											'CreatedAt', ad.""CreatedAt"",
											'CreatedBy', ad.""CreatedBy"",
											'UpdatedAt', ad.""UpdatedAt"",
											'UpdatedBy', ad.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""EcologySelections"" ad
									WHERE ad.""EntityId"" = fp.""Id""
								),
								'CreatedByUser', jsonb_build_object(
									'Id', u.""Id"",
									'Username', u.""Username"",
									'Email', u.""Email""
								),
								'UpdatedByUser', (
									SELECT jsonb_build_object(
										'Id', u.""Id"",
										'Username', u.""Username"",
										'Email', u.""Email""
									)
									FROM ""Users"" u
									WHERE u.""Id"" = fp.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""FirePrescriptions"" fp
						JOIN ""Locations"" loc ON loc.""Id"" = fp.""LocalAreaNameId""
						JOIN ""Parks"" p ON p.""Id"" = fp.""ParkId""
						JOIN ""Users"" u ON u.""Id"" = fp.""CreatedBy""
						WHERE fp.""Id"" = fire_prescription_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_fire_prescription_by_id: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_fire_prescription(
					input_data jsonb,
					fire_prescription_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_fire_prescription_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'FirePrescription';
						v_array_element TEXT;
						v_park_id UUID;
					BEGIN 
						-- Update FirePrescription data
						BEGIN
							-- Convert v_fire_prescription_id parameter to UUID
							v_fire_prescription_id := fire_prescription_id::UUID;

							-- Get ParkId
							SELECT ""ParkId"" INTO v_park_id
							FROM ""Locations""
							WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
							
							UPDATE public.""FirePrescriptions""
							SET
								""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
								""ParkId"" = v_park_id,
								""ParkArea"" =  (input_data->>'ParkArea')::NUMERIC,
								""PlannedArea"" =  (input_data->>'PlannedArea')::NUMERIC,
								""ActualBurntArea"" =  (input_data->>'ActualBurntArea')::NUMERIC,
								""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								""Remark"" = NULLIF(COALESCE(input_data->>'Remark', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_fire_prescription_id
							RETURNING ""Id"" INTO v_fire_prescription_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'FirePrescriptions table error: %', SQLERRM;
						END;

						-- Update FireObjectives Data
						IF input_data->'FireObjectives' IS NOT NULL AND jsonb_array_length(input_data->'FireObjectives') > 0 THEN
						
							-- Delete existing FireObjectives
							DELETE FROM public.""EcologySelections""
							WHERE ""EntityId"" = v_fire_prescription_id;
							
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'FireObjectives')
							LOOP
								BEGIN
									INSERT INTO public.""EcologySelections""(
										""Id"",
										""Value"",
										""FieldName"",
										""EntityId"",
										""EntityName"",
										""CreatedBy"", 
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'FireObjectives',
										v_fire_prescription_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (FireObjectives) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Return the updated v_fire_prescription_id
						RETURN v_fire_prescription_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_fire_prescription: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_fire_seminar(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_fire_seminar_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'FireSeminar';
						v_array_element TEXT;
						v_park_id UUID;
					BEGIN  
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Insert FireSeminars data
						BEGIN
							INSERT INTO public.""FireSeminars""(
								""Id"", 
								""LocalAreaNameId"",
								""ParkId"",
								""Participant"", 
								""Facilitator"",
								""Coordinates"",
								""Remark"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'LocalAreaNameId')::UUID,
								v_park_id,
								(input_data->>'Participant')::NUMERIC,
								input_data->>'Facilitator',
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								NULLIF(COALESCE(input_data->>'Remark', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_fire_seminar_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'FireSeminars table error: %', SQLERRM;
						END;

						-- Insert TargetedGroups Data
						IF input_data->'TargetedGroups' IS NOT NULL AND jsonb_array_length(input_data->'TargetedGroups') > 0 THEN
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'TargetedGroups')
							LOOP
								BEGIN
									INSERT INTO public.""EcologySelections""(
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
										'TargetedGroups',
										v_fire_seminar_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (TargetedGroups) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return the created v_fire_seminar_id
						RETURN v_fire_seminar_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_fire_seminar: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_fire_seminar(
					input_data jsonb,
					fire_seminar_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_fire_seminar_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'FireSeminar';
						v_array_element TEXT;
						v_park_id UUID;
					BEGIN 
						-- Update FireSeminar data
						BEGIN
							-- Convert v_fire_seminar_id parameter to UUID
							v_fire_seminar_id := fire_seminar_id::UUID;

							-- Get ParkId
							SELECT ""ParkId"" INTO v_park_id
							FROM ""Locations""
							WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
							
							UPDATE public.""FireSeminars""
							SET
								""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
								""ParkId"" = v_park_id,
								""Participant"" =  (input_data->>'Participant')::NUMERIC,
								""Facilitator"" =  input_data->>'Facilitator',
								""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								""Remark"" = NULLIF(COALESCE(input_data->>'Remark', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_fire_seminar_id
							RETURNING ""Id"" INTO v_fire_seminar_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'FireSeminars table error: %', SQLERRM;
						END;

						-- Update TargetedGroups Data
						IF input_data->'TargetedGroups' IS NOT NULL AND jsonb_array_length(input_data->'TargetedGroups') > 0 THEN
						
							-- Delete existing TargetedGroups
							DELETE FROM public.""EcologySelections""
							WHERE ""EntityId"" = v_fire_seminar_id;
							
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'TargetedGroups')
							LOOP
								BEGIN
									INSERT INTO public.""EcologySelections""(
										""Id"",
										""Value"",
										""FieldName"",
										""EntityId"",
										""EntityName"",
										""CreatedBy"", 
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'TargetedGroups',
										v_fire_seminar_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (TargetedGroups) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Return the updated v_fire_seminar_id
						RETURN v_fire_seminar_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_fire_seminar: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_fire_seminar_by_id(
					fire_seminar_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_entity_name CONSTANT VARCHAR(50) := 'FireSeminar';
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', f.""Id"",
								'LocalAreaNameId', f.""LocalAreaNameId"",
								'ParkId', f.""ParkId"",
								'Participant', f.""Participant"",
								'Facilitator', f.""Facilitator"",
								'Coordinates', f.""Coordinates"",
								'Remark', f.""Remark"",
								'CreatedBy', f.""CreatedBy"",
								'CreatedAt', f.""CreatedAt"",
								'UpdatedBy', f.""UpdatedBy"",
								'UpdatedAt', f.""UpdatedAt"",
								'Location', jsonb_build_object(
									'Id', loc.""Id"",
									'Name', loc.""Name"",
									'ParkId', loc.""ParkId"",
									'CreatedBy', loc.""CreatedBy"",
									'CreatedAt', loc.""CreatedAt""
								),
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'TargetedGroups', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', ad.""Id"",
											'Value', ad.""Value"",
											'FieldName', ad.""FieldName"",
											'EntityId', ad.""EntityId"",
											'EntityName', ad.""EntityName"",
											'CreatedAt', ad.""CreatedAt"",
											'CreatedBy', ad.""CreatedBy"",
											'UpdatedAt', ad.""UpdatedAt"",
											'UpdatedBy', ad.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""EcologySelections"" ad
									WHERE ad.""EntityId"" = f.""Id""
								),
								'CreatedByUser', jsonb_build_object(
									'Id', u.""Id"",
									'Username', u.""Username"",
									'Email', u.""Email""
								),
								'UpdatedByUser', (
									SELECT jsonb_build_object(
										'Id', u.""Id"",
										'Username', u.""Username"",
										'Email', u.""Email""
									)
									FROM ""Users"" u
									WHERE u.""Id"" = f.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""FireSeminars"" f
						JOIN ""Locations"" loc ON loc.""Id"" = f.""LocalAreaNameId""
						JOIN ""Parks"" p ON p.""Id"" = f.""ParkId""
						JOIN ""Users"" u ON u.""Id"" = f.""CreatedBy""
						WHERE f.""Id"" = fire_seminar_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_fire_seminar_by_id: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_firebreak(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_firebreak_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'FireBreak';
						v_array_element TEXT;
						v_park_id UUID;
					BEGIN  
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Insert FireBreaks data
						BEGIN
							INSERT INTO public.""FireBreaks""(
								""Id"", 
								""LocalAreaNameId"",
								""ParkId"",
								""Width"", 
								""Length"",
								""Coordinates"",
								""Remark"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'LocalAreaNameId')::UUID,
								v_park_id,
								(input_data->>'Width')::NUMERIC,
								(input_data->>'Length')::NUMERIC,
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								NULLIF(COALESCE(input_data->>'Remark', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_firebreak_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'FireBreaks table error: %', SQLERRM;
						END;

						-- Insert ClearanceMethods Data
						IF input_data->'ClearanceMethods' IS NOT NULL AND jsonb_array_length(input_data->'ClearanceMethods') > 0 THEN
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'ClearanceMethods')
							LOOP
								BEGIN
									INSERT INTO public.""EcologySelections""(
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
										'ClearanceMethods',
										v_firebreak_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (ClearanceMethods) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return the created v_firebreak_id
						RETURN v_firebreak_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_firebreak: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_firebreak_by_id(
					firebreak_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_entity_name CONSTANT VARCHAR(50) := 'FireBreak';
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', fb.""Id"",
								'LocalAreaNameId', fb.""LocalAreaNameId"",
								'ParkId', fb.""ParkId"",
								'Length', fb.""Length"",
								'Width', fb.""Width"",
								'Coordinates', fb.""Coordinates"",
								'Remark', fb.""Remark"",
								'CreatedBy', fb.""CreatedBy"",
								'CreatedAt', fb.""CreatedAt"",
								'UpdatedBy', fb.""UpdatedBy"",
								'UpdatedAt', fb.""UpdatedAt"",
								'Location', jsonb_build_object(
									'Id', loc.""Id"",
									'Name', loc.""Name"",
									'ParkId', loc.""ParkId"",
									'CreatedBy', loc.""CreatedBy"",
									'CreatedAt', loc.""CreatedAt""
								),
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'ClearanceMethods', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', ad.""Id"",
											'Value', ad.""Value"",
											'FieldName', ad.""FieldName"",
											'EntityId', ad.""EntityId"",
											'EntityName', ad.""EntityName"",
											'CreatedAt', ad.""CreatedAt"",
											'CreatedBy', ad.""CreatedBy"",
											'UpdatedAt', ad.""UpdatedAt"",
											'UpdatedBy', ad.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""EcologySelections"" ad
									WHERE ad.""EntityId"" = fb.""Id""
								),
								'CreatedByUser', jsonb_build_object(
									'Id', u.""Id"",
									'Username', u.""Username"",
									'Email', u.""Email""
								),
								'UpdatedByUser', (
									SELECT jsonb_build_object(
										'Id', u.""Id"",
										'Username', u.""Username"",
										'Email', u.""Email""
									)
									FROM ""Users"" u
									WHERE u.""Id"" = fb.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""FireBreaks"" fb
						JOIN ""Locations"" loc ON loc.""Id"" = fb.""LocalAreaNameId""
						JOIN ""Parks"" p ON p.""Id"" = fb.""ParkId""
						JOIN ""Users"" u ON u.""Id"" = fb.""CreatedBy""
						WHERE fb.""Id"" = firebreak_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_firebreak_by_id: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_firebreak(
					input_data jsonb,
					firebreak_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_firebreak_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'FireBreak';
						v_array_element TEXT;
						v_park_id UUID;
					BEGIN 
						-- Update FireBreak data
						BEGIN
							-- Convert v_firebreak_id parameter to UUID
							v_firebreak_id := firebreak_id::UUID;

							-- Get ParkId
							SELECT ""ParkId"" INTO v_park_id
							FROM ""Locations""
							WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
							
							UPDATE public.""FireBreaks""
							SET
								""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
								""ParkId"" = v_park_id,
								""Width"" =  (input_data->>'Width')::NUMERIC,
								""Length"" =  (input_data->>'Length')::NUMERIC,
								""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								""Remark"" = NULLIF(COALESCE(input_data->>'Remark', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_firebreak_id
							RETURNING ""Id"" INTO v_firebreak_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'FireBreaks table error: %', SQLERRM;
						END;

						-- Update ClearanceMethods Data
						IF input_data->'ClearanceMethods' IS NOT NULL AND jsonb_array_length(input_data->'ClearanceMethods') > 0 THEN
						
							-- Delete existing ClearanceMethods
							DELETE FROM public.""EcologySelections""
							WHERE ""EntityId"" = v_firebreak_id;
							
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'ClearanceMethods')
							LOOP
								BEGIN
									INSERT INTO public.""EcologySelections""(
										""Id"",
										""Value"",
										""FieldName"",
										""EntityId"",
										""EntityName"",
										""CreatedBy"", 
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'ClearanceMethods',
										v_firebreak_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (ClearanceMethods) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Return the updated v_firebreak_id
						RETURN v_firebreak_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_firebreak: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_wildfire(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_wildfire_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'WildFire';
						v_array_element TEXT;
						v_park_id UUID;
					BEGIN  
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Insert WildFires data
						BEGIN
							INSERT INTO public.""WildFires""(
								""Id"", 
								""LocalAreaNameId"",
								""ParkId"",
								""SourceOfFire"",
								""BurntArea"", 
								""BurningDuration"",
								""ParticipantStaff"",
								""OtherParticipant"",
								""OtherFireSource"",
								""Coordinates"",
								""Remark"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'LocalAreaNameId')::UUID,
								v_park_id,
								input_data->>'SourceOfFire',
								(input_data->>'BurntArea')::NUMERIC,
								(input_data->>'BurningDuration')::NUMERIC,
								(input_data->>'ParticipantStaff')::NUMERIC,
								(input_data->>'OtherParticipant')::NUMERIC,
								NULLIF(COALESCE(input_data->>'OtherFireSource', ''),''),
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								NULLIF(COALESCE(input_data->>'Remark', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_wildfire_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'WildFires table error: %', SQLERRM;
						END;

						-- Insert SourceOfInformation Data
						IF input_data->'SourceOfInformation' IS NOT NULL AND jsonb_array_length(input_data->'SourceOfInformation') > 0 THEN
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'SourceOfInformation')
							LOOP
								BEGIN
									INSERT INTO public.""EcologySelections""(
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
										'SourceOfInformation',
										v_wildfire_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (SourceOfInformation) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return the created v_wildfire_id
						RETURN v_wildfire_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_wildfire: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_wildfire(
					input_data jsonb,
					wildfire_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_wildfire_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'WildFire';
						v_array_element TEXT;
						v_park_id UUID;
					BEGIN 
						-- Update WildFire data
						BEGIN
							-- Convert v_wildfire_id parameter to UUID
							v_wildfire_id := wildfire_id::UUID;

							-- Get ParkId
							SELECT ""ParkId"" INTO v_park_id
							FROM ""Locations""
							WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
							
							UPDATE public.""WildFires""
							SET
								""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
								""ParkId"" = v_park_id,
								""SourceOfFire"" = input_data->>'SourceOfFire',
								""BurntArea"" =  (input_data->>'BurntArea')::NUMERIC,
								""BurningDuration"" =  (input_data->>'BurningDuration')::NUMERIC,
								""ParticipantStaff"" =  (input_data->>'ParticipantStaff')::NUMERIC,
								""OtherParticipant"" =  (input_data->>'OtherParticipant')::NUMERIC,
								""OtherFireSource"" = NULLIF(COALESCE(input_data->>'OtherFireSource', ''),''),
								""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								""Remark"" = NULLIF(COALESCE(input_data->>'Remark', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_wildfire_id
							RETURNING ""Id"" INTO v_wildfire_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'WildFires table error: %', SQLERRM;
						END;

						-- Update SourceOfInformation Data
						IF input_data->'SourceOfInformation' IS NOT NULL AND jsonb_array_length(input_data->'SourceOfInformation') > 0 THEN
						
							-- Delete existing SourceOfInformation
							DELETE FROM public.""EcologySelections""
							WHERE ""EntityId"" = v_wildfire_id;
							
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'SourceOfInformation')
							LOOP
								BEGIN
									INSERT INTO public.""EcologySelections""(
										""Id"",
										""Value"",
										""FieldName"",
										""EntityId"",
										""EntityName"",
										""CreatedBy"", 
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'SourceOfInformation',
										v_wildfire_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (SourceOfInformation) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Return the updated v_wildfire_id
						RETURN v_wildfire_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_wildfire: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_wildfire_by_id(
					wildfire_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_entity_name CONSTANT VARCHAR(50) := 'WildFire';
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', w.""Id"",
								'LocalAreaNameId', w.""LocalAreaNameId"",
								'ParkId', w.""ParkId"",
								'SourceOfFire', w.""SourceOfFire"",
								'BurntArea', w.""BurntArea"",
								'BurningDuration', w.""BurningDuration"",
								'ParticipantStaff', w.""ParticipantStaff"",
								'OtherParticipant', w.""OtherParticipant"",
								'OtherFireSource', w.""OtherFireSource"",
								'Coordinates', w.""Coordinates"",
								'Remark', w.""Remark"",
								'CreatedBy', w.""CreatedBy"",
								'CreatedAt', w.""CreatedAt"",
								'UpdatedBy', w.""UpdatedBy"",
								'UpdatedAt', w.""UpdatedAt"",
								'Location', jsonb_build_object(
									'Id', loc.""Id"",
									'Name', loc.""Name"",
									'ParkId', loc.""ParkId"",
									'CreatedBy', loc.""CreatedBy"",
									'CreatedAt', loc.""CreatedAt"",
									'Park', (
										SELECT jsonb_build_object(
											'Id', p.""Id"",
											'Name', p.""Name"",
											'Code', p.""Code"",
											'Zone', p.""Zone"",
											'CreatedAt', p.""CreatedAt"",
											'CreatedBy', p.""CreatedBy""
										)
										FROM ""Parks"" p
										WHERE p.""Id"" = loc.""ParkId""
									)
								),
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'SourceOfInformation', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', ad.""Id"",
											'Value', ad.""Value"",
											'FieldName', ad.""FieldName"",
											'EntityId', ad.""EntityId"",
											'EntityName', ad.""EntityName"",
											'CreatedAt', ad.""CreatedAt"",
											'CreatedBy', ad.""CreatedBy"",
											'UpdatedAt', ad.""UpdatedAt"",
											'UpdatedBy', ad.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""EcologySelections"" ad
									WHERE ad.""EntityId"" = w.""Id""
								),
								'CreatedByUser', jsonb_build_object(
									'Id', u.""Id"",
									'Username', u.""Username"",
									'Email', u.""Email""
								),
								'UpdatedByUser', (
									SELECT jsonb_build_object(
										'Id', u.""Id"",
										'Username', u.""Username"",
										'Email', u.""Email""
									)
									FROM ""Users"" u
									WHERE u.""Id"" = w.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""WildFires"" w
						JOIN ""Locations"" loc ON loc.""Id"" = w.""LocalAreaNameId""
						JOIN ""Parks"" p ON p.""Id"" = w.""ParkId""
						JOIN ""Users"" u ON u.""Id"" = w.""CreatedBy""
						WHERE w.""Id"" = wildfire_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_wildfire_by_id: %', SQLERRM;
					END;		
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_create_fire_prescription(jsonb);
				DROP FUNCTION IF EXISTS public.fn_fire_prescription_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_fire_prescription(jsonb, character varying);
				DROP FUNCTION IF EXISTS public.fn_create_fire_seminar(jsonb);
				DROP FUNCTION IF EXISTS public.fn_update_fire_seminar(jsonb, character varying);
				DROP FUNCTION IF EXISTS public.fn_fire_seminar_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_create_firebreak(jsonb);
				DROP FUNCTION IF EXISTS public.fn_firebreak_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_firebreak(jsonb, character varying);
				DROP FUNCTION IF EXISTS public.fn_create_wildfire(jsonb);
				DROP FUNCTION IF EXISTS public.fn_update_wildfire(jsonb, character varying);
				DROP FUNCTION IF EXISTS public.fn_wildfire_by_id(uuid);
            ");
        }
    }
}
