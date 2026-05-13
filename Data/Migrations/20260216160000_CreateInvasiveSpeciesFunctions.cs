using conservation_backend.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20260216160000_CreateInvasiveSpeciesFunctions")]
    public partial class CreateInvasiveSpeciesFunctions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_invasive_species(
					page_number integer,
					page_size integer,
					search_text character varying DEFAULT ''::character varying,
					activity_type character varying DEFAULT ''::character varying,
					park_id character varying DEFAULT ''::character varying,
					park_ids uuid[] DEFAULT NULL::uuid[]
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						total_count INTEGER;
						offset_value INTEGER;
						response_data JSONB;
						min_row_num INTEGER;
						max_row_num INTEGER;
					BEGIN
						-- Calculate offset and row ranges
						offset_value := (page_number - 1) * page_size;
						min_row_num := offset_value + 1;
						max_row_num := offset_value + page_size;
						
						-- invasive_species_data
						WITH invasive_species_data AS (
							SELECT 
								inv.""Id"",
								inv.""LocalAreaNameId"",
								inv.""ActivityType"",
								inv.""CreatedAt"",
								inv.""CreatedBy"",
								loc.""Id"" AS location_id,
								loc.""Name"" AS location_name,
								p.""Id"" AS parkId,
								p.""Name"" AS parkName,
								sp.""Id"" AS species_id,
								sp.""CommonName"" AS species_common_name,
								sp.""ScientificName"" AS species_scientific_name,
								u.""Id"" AS user_id,
								u.""Username"" AS username,
								ROW_NUMBER() OVER (ORDER BY inv.""CreatedAt"" DESC) AS row_number,
								-- Get total count
								COUNT(*) OVER() AS full_count
							FROM ""InvasiveSpecies"" inv
							JOIN ""Locations"" loc ON loc.""Id"" = inv.""LocalAreaNameId""
							JOIN ""Parks"" p ON p.""Id"" = inv.""ParkId""
							JOIN ""Species"" sp ON sp.""Id"" = inv.""SpeciesId""
							JOIN ""Users"" u ON u.""Id"" = inv.""CreatedBy""
							WHERE
								(search_text IS NULL OR search_text = '' OR
								loc.""Name"" ILIKE '%' || search_text || '%' OR
								sp.""ScientificName"" ILIKE '%' || search_text || '%' OR
								sp.""CommonName"" ILIKE '%' || search_text || '%')
								AND inv.""DeletedAt"" IS NULL
								AND (park_id IS NULL OR park_id = '' OR inv.""ParkId"" = park_id::UUID)
								AND (activity_type IS NULL OR activity_type = '' OR inv.""ActivityType"" = activity_type)
								AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR
     							inv.""ParkId"" = ANY(park_ids))
						),
						paginated_data AS (
							SELECT * FROM invasive_species_data
							WHERE row_number BETWEEN min_row_num AND max_row_num
						)
						SELECT 
							jsonb_agg(
								jsonb_build_object(
									'RowNumber', row_number,
									'Id', ""Id"",
									'LocalAreaNameId', ""LocalAreaNameId"",
									'ActivityType', ""ActivityType"",
									'CreatedAt', ""CreatedAt"",
									'CreatedBy', ""CreatedBy"",
									'Location', jsonb_build_object(
										'Id', location_id,
										'Name', location_name
									),
									'Park', jsonb_build_object(
										'Id', parkId,
										'Name', parkName
									),
									'Species', jsonb_build_object(
										'Id', species_id,
										'CommonName', species_common_name,
										'ScientificName', species_scientific_name
									),
									'CreatedByUser', jsonb_build_object(
										'Id', user_id,
										'Username', username
									)
								)
							),
							-- Get total count
							MAX(full_count)
						INTO response_data, total_count
						FROM paginated_data;
						 
						-- object to return
						RETURN jsonb_build_object(
							'data', COALESCE(response_data, '[]'::jsonb),
							'meta', jsonb_build_object(
								'page', page_number,
								'pageSize', page_size,
								'totalItems', COALESCE(total_count, 0),
								'totalPages', CASE
									WHEN COALESCE(total_count, 0) = 0 THEN 0
									ELSE CEIL(COALESCE(total_count, 0)::DECIMAL / page_size)
								END
							)
						);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_invasive_species: %', SQLERRM;
					END;        
				$BODY$;
            ");

            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_invasive_species(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_invasive_species_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'InvasiveSpecies';
						v_other_specie_observed_element JSONB;
						v_array_element TEXT;
						v_park_id UUID;
						v_image_id TEXT;
					BEGIN  
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Insert InvasiveSpecies data
						BEGIN
							INSERT INTO public.""InvasiveSpecies""(
								""Id"", 
								""LocalAreaNameId"", 
								""ParkId"",
								""SpeciesId"", 
								""ActivityType"", 
								""ControlType"",
								""InfestedArea"", 
								""AreaCoverage"", 
								""BiologicalMethod"", 
								""BiologicalAgent"", 
								""OtherPossibleSource"", 
								""Coordinates"",
								""Remark"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'LocalAreaNameId')::UUID,
								v_park_id,
								(input_data->>'SpeciesId')::UUID,
								input_data->>'ActivityType',
								NULLIF(COALESCE(input_data->>'ControlType', ''),''),
								NULLIF(COALESCE(input_data->>'InfestedArea', ''),''),
								NULLIF(COALESCE(input_data->>'AreaCoverage', ''),''),
								NULLIF(COALESCE(input_data->>'BiologicalMethod', ''),''),
								NULLIF(COALESCE(input_data->>'BiologicalAgent', ''),''),
								NULLIF(COALESCE(input_data->>'OtherPossibleSource', ''),''),
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								NULLIF(COALESCE(input_data->>'Remark', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_invasive_species_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'InvasiveSpecies table error: %', SQLERRM;
						END;

						-- Insert Files Data
						BEGIN
							v_image_id := input_data->>'ImageId';
							IF v_image_id IS NOT NULL AND v_image_id != '' THEN
								UPDATE public.""Files""
								SET
									""EntityId"" = v_invasive_species_id,
									""EntityName"" = v_entity_name,
									""UpdatedBy"" = (input_data->>'CreatedBy')::UUID,
									""UpdatedAt"" = NOW()
								WHERE ""Id"" = v_image_id::UUID;
							END IF;
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'Files table error: %', SQLERRM;
						END;

						-- Insert PossibleSources Data
						IF input_data->'PossibleSources' IS NOT NULL AND jsonb_array_length(input_data->'PossibleSources') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'PossibleSources')
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
										'PossibleSources',
										v_invasive_species_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (PossibleSources) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert ChemicalMethods Data
						IF input_data->'ChemicalMethods' IS NOT NULL AND jsonb_array_length(input_data->'ChemicalMethods') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'ChemicalMethods')
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
										'ChemicalMethods',
										v_invasive_species_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (ChemicalMethods) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert MechanicalMethods Data
						IF input_data->'MechanicalMethods' IS NOT NULL AND jsonb_array_length(input_data->'MechanicalMethods') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'MechanicalMethods')
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
										'MechanicalMethods',
										v_invasive_species_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections (MechanicalMethods) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert OtherSpecieObserved Data
						IF input_data->'OtherSpecieObserved' IS NOT NULL AND jsonb_array_length(input_data->'OtherSpecieObserved') > 0 THEN
							FOR v_other_specie_observed_element IN
								SELECT * FROM jsonb_array_elements(input_data->'OtherSpecieObserved')
							LOOP
								BEGIN
									INSERT INTO public.""SpeciesOccurrences""(
										""Id"",
										""SpeciesId"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_other_specie_observed_element->>'SpeciesId')::UUID,
										v_invasive_species_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'SpeciesOccurrences table error: %', SQLERRM;
								END;
							END LOOP;
						END IF;
						
						-- Return the created v_invasive_species_id
						RETURN v_invasive_species_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_invasive_species: %', SQLERRM;
					END;		
				$BODY$;
			");

            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_invasive_species_by_id(
					invasive_species_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', invs.""Id"",
								'LocalAreaNameId', invs.""LocalAreaNameId"",
								'SpeciesId', invs.""SpeciesId"",
								'ActivityType', invs.""ActivityType"",
								'ControlType', invs.""ControlType"",
								'InfestedArea', invs.""InfestedArea"",
								'AreaCoverage', invs.""AreaCoverage"",
								'BiologicalMethod', invs.""BiologicalMethod"",
								'BiologicalAgent', invs.""BiologicalAgent"",
								'OtherPossibleSource', invs.""OtherPossibleSource"",
								'Coordinates', invs.""Coordinates"",
								'Remark', invs.""Remark"",
								'CreatedBy', invs.""CreatedBy"",
								'CreatedAt', invs.""CreatedAt"",
								'UpdatedAt', invs.""UpdatedAt"",
								'UpdatedBy', invs.""UpdatedBy"",
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
								'Species', jsonb_build_object(
									'Id', sp.""Id"",
									'CommonName', sp.""CommonName"",
									'ScientificName', sp.""ScientificName"",
									'Type', sp.""Type"",
									'CreatedAt', sp.""CreatedAt"",
									'CreatedBy', sp.""CreatedBy""
								),
								'SpeciesOccurrences', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', spo.""Id"",
											'SpeciesId', spo.""SpeciesId"",
											'EntityId', spo.""EntityId"",
											'EntityName', spo.""EntityName"",
											'CreatedAt', spo.""CreatedAt"",
											'CreatedBy', spo.""CreatedBy"",
											'Species', (
												SELECT jsonb_build_object(
													'Id', sp.""Id"",
													'CommonName', sp.""CommonName"",
													'ScientificName', sp.""ScientificName"",
													'Type', sp.""Type"",
													'CreatedAt', sp.""CreatedAt"",
													'CreatedBy', sp.""CreatedBy""
												)
												FROM ""Species"" sp
												WHERE sp.""Id"" = spo.""SpeciesId""
											)
										)
									), '[]'::jsonb)
									FROM ""SpeciesOccurrences"" spo
									WHERE spo.""EntityId"" = invs.""Id""
								),
								'ChemicalMethods', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', ec.""Id"",
											'Value', ec.""Value"",
											'FieldName', ec.""FieldName"",
											'EntityId', ec.""EntityId"",
											'EntityName', ec.""EntityName"",
											'CreatedAt', ec.""CreatedAt"",
											'CreatedBy', ec.""CreatedBy"",
											'UpdatedAt', ec.""UpdatedAt"",
											'UpdatedBy', ec.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""EcologySelections"" ec
									WHERE ec.""EntityId"" = invs.""Id""
									AND ec.""FieldName"" = 'ChemicalMethods'
								),
								'PossibleSources', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', ec.""Id"",
											'Value', ec.""Value"",
											'FieldName', ec.""FieldName"",
											'EntityId', ec.""EntityId"",
											'EntityName', ec.""EntityName"",
											'CreatedAt', ec.""CreatedAt"",
											'CreatedBy', ec.""CreatedBy"",
											'UpdatedAt', ec.""UpdatedAt"",
											'UpdatedBy', ec.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""EcologySelections"" ec
									WHERE ec.""EntityId"" = invs.""Id""
									AND ec.""FieldName"" = 'PossibleSources'
								),
								'MechanicalMethods', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', ec.""Id"",
											'Value', ec.""Value"",
											'FieldName', ec.""FieldName"",
											'EntityId', ec.""EntityId"",
											'EntityName', ec.""EntityName"",
											'CreatedAt', ec.""CreatedAt"",
											'CreatedBy', ec.""CreatedBy"",
											'UpdatedAt', ec.""UpdatedAt"",
											'UpdatedBy', ec.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""EcologySelections"" ec
									WHERE ec.""EntityId"" = invs.""Id""
									AND ec.""FieldName"" = 'MechanicalMethods'
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
									WHERE u.""Id"" = invs.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""InvasiveSpecies"" invs
						JOIN ""Locations"" loc ON loc.""Id"" = invs.""LocalAreaNameId""
						JOIN ""Species"" sp ON sp.""Id"" = invs.""SpeciesId""
						JOIN ""Users"" u ON u.""Id"" = invs.""CreatedBy""
						WHERE invs.""Id"" = invasive_species_id;
						
						-- data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_invasive_species_by_id: %', SQLERRM;
					END;		
				$BODY$;
            ");

            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_invasive_species(
					input_data jsonb,
					invasive_species_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE
				    v_invasive_species_id UUID;
				    v_entity_name CONSTANT VARCHAR(50) := 'InvasiveSpecies';
				    v_other_specie_observed_element JSONB;
				    v_array_element TEXT;
					v_park_id UUID;
					v_image_id TEXT;
				BEGIN 
				    -- Update InvasiveSpecies data
				    BEGIN
				        -- Convert vegetation_id parameter to UUID
				        v_invasive_species_id := invasive_species_id::UUID;

						-- Get ParkId
						SELECT ""ParkId"" INTO v_park_id 
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
							
				        UPDATE public.""InvasiveSpecies""
				        SET
				            ""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
							""ParkId"" = v_park_id,
				            ""SpeciesId"" =  (input_data->>'SpeciesId')::UUID,
				            ""ActivityType"" = input_data->>'ActivityType',
				            ""ControlType"" = NULLIF(COALESCE(input_data->>'ControlType', ''),''),
				            ""InfestedArea"" = NULLIF(COALESCE(input_data->>'InfestedArea', ''),''),
				            ""AreaCoverage"" = NULLIF(COALESCE(input_data->>'AreaCoverage', ''),''),
				            ""BiologicalMethod"" = NULLIF(COALESCE(input_data->>'BiologicalMethod', ''),''),
				            ""BiologicalAgent"" = NULLIF(COALESCE(input_data->>'BiologicalAgent', ''),''),
				            ""OtherPossibleSource"" = NULLIF(COALESCE(input_data->>'OtherPossibleSource', ''),''),
				            ""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
				            ""Remark"" = NULLIF(COALESCE(input_data->>'Remark', ''),''),
				            ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
				            ""UpdatedAt"" = NOW()
				        WHERE ""Id"" = v_invasive_species_id
				        RETURNING ""Id"" INTO v_invasive_species_id;
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'InvasiveSpecies table error: %', SQLERRM;
				    END;

					-- Insert Files Data
					BEGIN
						v_image_id := input_data->>'ImageId';
						IF v_image_id IS NOT NULL AND v_image_id != '' THEN
							UPDATE public.""Files""
							SET
								""EntityId"" = v_invasive_species_id,
								""EntityName"" = v_entity_name,
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_image_id::UUID;
						END IF;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'Files table error: %', SQLERRM;
					END;
				    
				    -- Update PossibleSources Data
				    BEGIN
				        IF input_data->'PossibleSources' IS NOT NULL AND jsonb_array_length(input_data->'PossibleSources') > 0 THEN
				        
				            -- Delete existing PossibleSources
				            DELETE FROM public.""EcologySelections""
				            WHERE ""EntityId"" = v_invasive_species_id 
				            AND ""EntityName"" = v_entity_name 
				            AND ""FieldName"" = 'PossibleSources';
				            
				            -- Insert PossibleSources
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'PossibleSources')
				            LOOP
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
				                    'PossibleSources',
				                    v_invasive_species_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        ELSE
				            -- Only delete if there are existing PossibleSources
				            IF EXISTS (
				                SELECT 1 FROM public.""EcologySelections"" 
				                WHERE ""EntityId"" = v_invasive_species_id 
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'PossibleSources'
				            ) THEN
				                DELETE FROM public.""EcologySelections""
				                WHERE ""EntityId"" = v_invasive_species_id 
				                AND ""EntityName"" = v_entity_name 
				                AND ""FieldName"" = 'PossibleSources';
				            END IF;
				        END IF;	
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'EcologySelections (PossibleSources) table error: %', SQLERRM;
				    END;

				    -- Update ChemicalMethods Data
				    BEGIN
				        IF input_data->'ChemicalMethods' IS NOT NULL AND jsonb_array_length(input_data->'ChemicalMethods') > 0 THEN
				        
				            -- Delete existing ChemicalMethods
				            DELETE FROM public.""EcologySelections""
				            WHERE ""EntityId"" = v_invasive_species_id 
				            AND ""EntityName"" = v_entity_name 
				            AND ""FieldName"" = 'ChemicalMethods';
				            
				            -- Insert ChemicalMethods
				            FOR v_array_element IN
				                SELECT jsonb_array_elements_text(input_data->'ChemicalMethods')
				            LOOP
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
				                    'ChemicalMethods',
				                    v_invasive_species_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        ELSE
				            -- Only delete if there are existing ChemicalMethods
				            IF EXISTS (
				                SELECT 1 FROM public.""EcologySelections"" 
				                WHERE ""EntityId"" = v_invasive_species_id 
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'ChemicalMethods'
				            ) THEN
				                DELETE FROM public.""EcologySelections""
				                WHERE ""EntityId"" = v_invasive_species_id 
				                AND ""EntityName"" = v_entity_name 
				                AND ""FieldName"" = 'ChemicalMethods';
				            END IF;
				        END IF;	
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'EcologySelections (ChemicalMethods) table error: %', SQLERRM;
				    END;
				    
				    -- Update MechanicalMethods Data
				    BEGIN
				        IF input_data->'MechanicalMethods' IS NOT NULL AND jsonb_array_length(input_data->'MechanicalMethods') > 0 THEN
				        
				            -- Delete existing MechanicalMethods
				            DELETE FROM public.""EcologySelections""
				            WHERE ""EntityId"" = v_invasive_species_id 
				            AND ""EntityName"" = v_entity_name 
				            AND ""FieldName"" = 'MechanicalMethods';
				            
				            -- Insert MechanicalMethods
				            FOR v_array_element IN
				                SELECT jsonb_array_elements_text(input_data->'MechanicalMethods')
				            LOOP
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
				                    'MechanicalMethods',
				                    v_invasive_species_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        ELSE
				            -- Only delete if there are existing MechanicalMethods
				            IF EXISTS (
				                SELECT 1 FROM public.""EcologySelections"" 
				                WHERE ""EntityId"" = v_invasive_species_id 
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'MechanicalMethods'
				            ) THEN
				                DELETE FROM public.""EcologySelections""
				                WHERE ""EntityId"" = v_invasive_species_id 
				                AND ""EntityName"" = v_entity_name 
				                AND ""FieldName"" = 'MechanicalMethods';
				            END IF;
				        END IF;	
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'EcologySelections (MechanicalMethods) table error: %', SQLERRM;
				    END;

				    -- Update OtherSpecieObserved Data
				    BEGIN
				        IF input_data->'OtherSpecieObserved' IS NOT NULL AND jsonb_array_length(input_data->'OtherSpecieObserved') > 0 THEN
				            
				            -- Delete existing OtherSpecieObserved
				            DELETE FROM public.""SpeciesOccurrences""
				            WHERE ""EntityId"" = v_invasive_species_id;
				                
				            -- Insert OtherSpecieObserved data	
				            FOR v_other_specie_observed_element IN
				                SELECT * FROM jsonb_array_elements(input_data->'OtherSpecieObserved')
				            LOOP
				                INSERT INTO public.""SpeciesOccurrences""(
				                    ""Id"",
				                    ""SpeciesId"",
				                    ""EntityId"", 
				                    ""EntityName"", 
				                    ""CreatedBy"",
				                    ""CreatedAt"",
				                    ""UpdatedBy"",
				                    ""UpdatedAt""
				                ) VALUES (
				                    gen_random_uuid(),
				                    (v_other_specie_observed_element->>'SpeciesId')::UUID,
				                    v_invasive_species_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        ELSE
				            -- Only delete if there are existing SpeciesOccurrences
				            IF EXISTS (
				                SELECT 1 FROM public.""SpeciesOccurrences"" 
				                WHERE ""EntityId"" = v_invasive_species_id 
				                AND ""EntityName"" = v_entity_name
				            ) THEN
				                DELETE FROM public.""SpeciesOccurrences"" 
				                WHERE ""EntityId"" = v_invasive_species_id 
				                AND ""EntityName"" = v_entity_name;
				            END IF;
				        END IF;
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'SpeciesOccurrences table error: %', SQLERRM;
				    END;

				    -- Return the updated invasive_species_id
				    RETURN v_invasive_species_id::TEXT;
				    
				EXCEPTION
				    WHEN OTHERS THEN
				        RAISE EXCEPTION 'Error in fn_update_invasive_species: %', SQLERRM;
				END;        
				$BODY$;
			");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS public.fn_invasive_species(integer, integer, character varying, character varying, character varying, uuid[]);
                DROP FUNCTION IF EXISTS public.fn_create_invasive_species(jsonb);
                DROP FUNCTION IF EXISTS public.fn_invasive_species_by_id(uuid);
                DROP FUNCTION IF EXISTS public.fn_update_invasive_species(jsonb, character varying);
            ");
        }
    }
}
