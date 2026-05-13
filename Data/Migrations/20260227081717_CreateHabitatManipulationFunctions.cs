using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateHabitatManipulationFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_habitat_manipulations(
					page_number integer,
					page_size integer,
					search_text character varying DEFAULT ''::character varying,
					action_taken character varying DEFAULT ''::character varying,
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
						
						-- habitat_manipulation_data
						WITH habitat_manipulation_data AS (
							SELECT 
								hm.""Id"",
								hm.""LocalAreaNameId"",
								hm.""CreatedAt"",
								hm.""CreatedBy"",
								hm.""AreaCovered"",
								hm.""ActionTaken"",
								loc.""Id"" AS location_id,
								loc.""Name"" AS location_name,
								p.""Id"" AS parkId,
								p.""Name"" AS parkName,
								sp.""Id"" AS species_id,
								sp.""CommonName"" AS species_common_name,
								sp.""ScientificName"" AS species_scientific_name,
								u.""Id"" AS user_id,
								u.""Username"" AS username,
								ROW_NUMBER() OVER (ORDER BY hm.""CreatedAt"" DESC) AS row_number,
								-- Get total count
								COUNT(*) OVER() AS full_count
							FROM ""HabitatManipulations"" hm
							JOIN ""Locations"" loc ON loc.""Id"" = hm.""LocalAreaNameId""
							JOIN ""Parks"" p ON p.""Id"" = hm.""ParkId""
							JOIN ""Species"" sp ON sp.""Id"" = hm.""SpeciesId""
							JOIN ""Users"" u ON u.""Id"" = hm.""CreatedBy""
							WHERE
								(search_text IS NULL OR search_text = '' OR
								loc.""Name"" ILIKE '%' || search_text || '%' OR
								sp.""ScientificName"" ILIKE '%' || search_text || '%' OR
								sp.""CommonName"" ILIKE '%' || search_text || '%')
							AND hm.""DeletedAt"" IS NULL
							AND (action_taken IS NULL OR action_taken = '' OR hm.""ActionTaken"" = action_taken)
							AND (park_id IS NULL OR park_id = '' OR hm.""ParkId"" = park_id::UUID)
							AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR
     							hm.""ParkId"" = ANY(park_ids))
						),
						paginated_data AS (
							SELECT * FROM habitat_manipulation_data
							WHERE row_number BETWEEN min_row_num AND max_row_num
						)
						SELECT 
							jsonb_agg(
								jsonb_build_object(
									'RowNumber', row_number,
									'Id', ""Id"",
									'LocalAreaNameId', ""LocalAreaNameId"",
									'CreatedAt', ""CreatedAt"",
									'CreatedBy', ""CreatedBy"",
									'AreaCovered', ""AreaCovered"",
									'ActionTaken', ""ActionTaken"",
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
							RAISE EXCEPTION 'Error in fn_habitat_manipulations: %', SQLERRM;
					END;        
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_habitat_manipulation(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
				    DECLARE
				        v_habitat_manipulation_id UUID;
				        v_entity_name CONSTANT VARCHAR(50) := 'HabitatManipulation';
				        v_array_element TEXT;
						v_park_id UUID;
				    BEGIN  
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
				        -- Insert HabitatManipulations data
				        BEGIN
				            INSERT INTO public.""HabitatManipulations""(
				                ""Id"", 
				                ""LocalAreaNameId"", 
								""ParkId"",
								""SpeciesId"", 
								""Session"", 
								""Environment"", 
								""AreaCovered"", 
								""ActionTaken"", 
								""TotalNumber"", 
								""SourceOfSeedling"", 
								""ExpectedOutput"",
				                ""Coordinates"",
				                ""Remark"",
				                ""CreatedBy"", 
				                ""CreatedAt""
				            ) VALUES (
				                gen_random_uuid(),
				                (input_data->>'LocalAreaNameId')::UUID,
								v_park_id,
				                (input_data->>'SpeciesId')::UUID,
				                input_data->>'Session',
								input_data->>'Environment',
								(input_data->>'AreaCovered')::NUMERIC,
								input_data->>'ActionTaken',
								(input_data->>'TotalNumber')::NUMERIC,
				                NULLIF(COALESCE(input_data->>'SourceOfSeedling', ''),''),
								input_data->>'ExpectedOutput',
				                NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
				                NULLIF(COALESCE(input_data->>'Remark', ''),''),
				                (input_data->>'CreatedBy')::UUID,
				                NOW()
				            ) RETURNING ""Id"" INTO v_habitat_manipulation_id;
				        EXCEPTION
				            WHEN OTHERS THEN
				                RAISE EXCEPTION 'HabitatManipulations table error: %', SQLERRM;
				        END;

				        -- Insert Habitats Data
				        IF input_data->'Habitats' IS NOT NULL AND jsonb_array_length(input_data->'Habitats') > 0 THEN
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'Habitats')
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
				                        'Habitats',
				                        v_habitat_manipulation_id,
				                        v_entity_name,
				                        (input_data->>'CreatedBy')::UUID,
				                        NOW()
				                    );
				                EXCEPTION
				                    WHEN OTHERS THEN
				                        RAISE EXCEPTION 'EcologySelections (Habitats) table error: %', SQLERRM;
				                END;    
				            END LOOP;
				        END IF;

				        -- Insert ManipulationActivities Data
				        IF input_data->'ManipulationActivities' IS NOT NULL AND jsonb_array_length(input_data->'ManipulationActivities') > 0 THEN
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'ManipulationActivities')
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
				                        'ManipulationActivities',
				                        v_habitat_manipulation_id,
				                        v_entity_name,
				                        (input_data->>'CreatedBy')::UUID,
				                        NOW()
				                    );
				                EXCEPTION
				                    WHEN OTHERS THEN
				                        RAISE EXCEPTION 'EcologySelections (ManipulationActivities) table error: %', SQLERRM;
				                END;
				            END LOOP;
				        END IF;

				        -- Insert NaturalCauses Data
				        IF input_data->'NaturalCauses' IS NOT NULL AND jsonb_array_length(input_data->'NaturalCauses') > 0 THEN
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'NaturalCauses')
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
				                        'NaturalCauses',
				                        v_habitat_manipulation_id,
				                        v_entity_name,
				                        (input_data->>'CreatedBy')::UUID,
				                        NOW()
				                    );
				                EXCEPTION
				                    WHEN OTHERS THEN
				                        RAISE EXCEPTION 'EcologySelections (NaturalCauses) table error: %', SQLERRM;
				                END;    
				            END LOOP;
				        END IF;

						-- Insert ManMadeCauses Data
				        IF input_data->'ManMadeCauses' IS NOT NULL AND jsonb_array_length(input_data->'ManMadeCauses') > 0 THEN
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'ManMadeCauses')
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
				                        'ManMadeCauses',
				                        v_habitat_manipulation_id,
				                        v_entity_name,
				                        (input_data->>'CreatedBy')::UUID,
				                        NOW()
				                    );
				                EXCEPTION
				                    WHEN OTHERS THEN
				                        RAISE EXCEPTION 'EcologySelections (ManMadeCauses) table error: %', SQLERRM;
				                END;    
				            END LOOP;
				        END IF;

						-- Insert ManipulationDriver Data
				        IF input_data->'ManipulationDriver' IS NOT NULL AND jsonb_array_length(input_data->'ManipulationDriver') > 0 THEN
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'ManipulationDriver')
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
				                        'ManipulationDriver',
				                        v_habitat_manipulation_id,
				                        v_entity_name,
				                        (input_data->>'CreatedBy')::UUID,
				                        NOW()
				                    );
				                EXCEPTION
				                    WHEN OTHERS THEN
				                        RAISE EXCEPTION 'EcologySelections (ManipulationDriver) table error: %', SQLERRM;
				                END;    
				            END LOOP;
				        END IF;
				        
				        -- Return the created v_habitat_manipulation_id
				        RETURN v_habitat_manipulation_id::TEXT;
				        
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'Error in fn_create_habitat_manipulation: %', SQLERRM;
				    END;        
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_habitat_manipulation_by_id(
					habitat_manipulation_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', hm.""Id"",
								'LocalAreaNameId', hm.""LocalAreaNameId"",
								'SpeciesId', hm.""SpeciesId"",
								'ParkId', hm.""ParkId"",
								'Session', hm.""Session"",
								'Environment', hm.""Environment"",
								'AreaCovered', hm.""AreaCovered"",
								'ActionTaken', hm.""ActionTaken"",
								'TotalNumber', hm.""TotalNumber"",
								'SourceOfSeedling', hm.""SourceOfSeedling"",
								'Coordinates', hm.""Coordinates"",
								'ExpectedOutput', hm.""ExpectedOutput"",			
								'Coordinates', hm.""Coordinates"",
								'Remark', hm.""Remark"",
								'CreatedBy', hm.""CreatedBy"",
								'CreatedAt', hm.""CreatedAt"",
								'UpdatedAt', hm.""UpdatedAt"",
								'UpdatedBy', hm.""UpdatedBy"",
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
								'Species', jsonb_build_object(
									'Id', sp.""Id"",
									'CommonName', sp.""CommonName"",
									'ScientificName', sp.""ScientificName"",
									'Type', sp.""Type"",
									'CreatedAt', sp.""CreatedAt"",
									'CreatedBy', sp.""CreatedBy""
								),
								'Habitats', (
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
									WHERE ec.""EntityId"" = hm.""Id""
									AND ec.""FieldName"" = 'Habitats'
								),
								'ManipulationActivities', (
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
									WHERE ec.""EntityId"" = hm.""Id""
									AND ec.""FieldName"" = 'ManipulationActivities'
								),
								'NaturalCauses', (
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
									WHERE ec.""EntityId"" = hm.""Id""
									AND ec.""FieldName"" = 'NaturalCauses'
								),
								'ManMadeCauses', (
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
									WHERE ec.""EntityId"" = hm.""Id""
									AND ec.""FieldName"" = 'ManMadeCauses'
								),
								'ManipulationDriver', (
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
									WHERE ec.""EntityId"" = hm.""Id""
									AND ec.""FieldName"" = 'ManipulationDriver'
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
									WHERE u.""Id"" = hm.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""HabitatManipulations"" hm
						JOIN ""Locations"" loc ON loc.""Id"" = hm.""LocalAreaNameId""
						JOIN ""Species"" sp ON sp.""Id"" = hm.""SpeciesId""
						JOIN ""Parks"" p ON p.""Id"" = hm.""ParkId""
						JOIN ""Users"" u ON u.""Id"" = hm.""CreatedBy""
						WHERE hm.""Id"" = habitat_manipulation_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_habitat_manipulation_by_id: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_habitat_manipulation(
					input_data jsonb,
					habitat_manipulation_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE
				    v_habitat_manipulation_id UUID;
				    v_entity_name CONSTANT VARCHAR(50) := 'HabitatManipulation';
				    v_array_element TEXT;
					v_park_id UUID;
				BEGIN 
				    -- Update HabitatManipulation data
				    BEGIN
				        -- Convert habitat_manipulation_id parameter to UUID
				        v_habitat_manipulation_id := habitat_manipulation_id::UUID;

						-- Get ParkId
						SELECT ""ParkId"" INTO v_park_id 
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
							
				        UPDATE public.""HabitatManipulations""
				        SET
				            ""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
							""ParkId"" = v_park_id,
				            ""SpeciesId"" =  (input_data->>'SpeciesId')::UUID,
				            ""Session"" = input_data->>'Session',
							""Environment"" = input_data->>'Environment',
							""AreaCovered"" = (input_data->>'AreaCovered')::NUMERIC,
							""ActionTaken"" = input_data->>'ActionTaken',
							""TotalNumber"" = (input_data->>'TotalNumber')::NUMERIC,
							""SourceOfSeedling"" = NULLIF(COALESCE(input_data->>'SourceOfSeedling', ''),''),
							""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
							""ExpectedOutput"" = input_data->>'ExpectedOutput',
							""Remark"" = input_data->>'Remark',
				            ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
				            ""UpdatedAt"" = NOW()
				        WHERE ""Id"" = v_habitat_manipulation_id
				        RETURNING ""Id"" INTO v_habitat_manipulation_id;
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'HabitatManipulations table error: %', SQLERRM;
				    END;
				    
				    -- Update Habitats Data
				    BEGIN
				        IF input_data->'Habitats' IS NOT NULL AND jsonb_array_length(input_data->'Habitats') > 0 THEN
				        
				            -- Delete existing Habitats
				            DELETE FROM public.""EcologySelections""
				            WHERE ""EntityId"" = v_habitat_manipulation_id 
				            AND ""EntityName"" = v_entity_name 
				            AND ""FieldName"" = 'Habitats';
				            
				            -- Insert Habitats
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'Habitats')
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
				                    'Habitats',
				                    v_habitat_manipulation_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        ELSE
				            -- Only delete if there are existing Habitats
				            IF EXISTS (
				                SELECT 1 FROM public.""EcologySelections"" 
				                WHERE ""EntityId"" = v_habitat_manipulation_id 
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'Habitats'
				            ) THEN
				                DELETE FROM public.""EcologySelections""
				                WHERE ""EntityId"" = v_habitat_manipulation_id 
				                AND ""EntityName"" = v_entity_name 
				                AND ""FieldName"" = 'Habitats';
				            END IF;
				        END IF;	
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'EcologySelections (Habitats) table error: %', SQLERRM;
				    END;

					-- Update ManipulationActivities Data
				    BEGIN
				        IF input_data->'ManipulationActivities' IS NOT NULL AND jsonb_array_length(input_data->'ManipulationActivities') > 0 THEN
				        
				            -- Delete existing ManipulationActivities
				            DELETE FROM public.""EcologySelections""
				            WHERE ""EntityId"" = v_habitat_manipulation_id 
				            AND ""EntityName"" = v_entity_name 
				            AND ""FieldName"" = 'ManipulationActivities';
				            
				            -- Insert ManipulationActivities
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'ManipulationActivities')
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
				                    'ManipulationActivities',
				                    v_habitat_manipulation_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        ELSE
				            -- Only delete if there are existing ManipulationActivities
				            IF EXISTS (
				                SELECT 1 FROM public.""EcologySelections"" 
				                WHERE ""EntityId"" = v_habitat_manipulation_id 
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'ManipulationActivities'
				            ) THEN
				                DELETE FROM public.""EcologySelections""
				                WHERE ""EntityId"" = v_habitat_manipulation_id
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'ManipulationActivities';
				            END IF;
				        END IF;	
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'EcologySelections (ManipulationActivities) table error: %', SQLERRM;
				    END;

					-- Update NaturalCauses Data
				    BEGIN
				        IF input_data->'NaturalCauses' IS NOT NULL AND jsonb_array_length(input_data->'NaturalCauses') > 0 THEN
				        
				            -- Delete existing NaturalCauses
				            DELETE FROM public.""EcologySelections""
				            WHERE ""EntityId"" = v_habitat_manipulation_id 
				            AND ""EntityName"" = v_entity_name 
				            AND ""FieldName"" = 'NaturalCauses';
				            
				            -- Insert NaturalCauses
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'NaturalCauses')
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
				                    'NaturalCauses',
				                    v_habitat_manipulation_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        ELSE
				            -- Only delete if there are existing NaturalCauses
				            IF EXISTS (
				                SELECT 1 FROM public.""EcologySelections"" 
				                WHERE ""EntityId"" = v_habitat_manipulation_id 
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'NaturalCauses'
				            ) THEN
				                DELETE FROM public.""EcologySelections""
				                WHERE ""EntityId"" = v_habitat_manipulation_id
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'NaturalCauses';
				            END IF;
				        END IF;	
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'EcologySelections (NaturalCauses) table error: %', SQLERRM;
				    END;

					-- Update ManMadeCauses Data
				    BEGIN
				        IF input_data->'ManMadeCauses' IS NOT NULL AND jsonb_array_length(input_data->'ManMadeCauses') > 0 THEN
				        
				            -- Delete existing ManMadeCauses
				            DELETE FROM public.""EcologySelections""
				            WHERE ""EntityId"" = v_habitat_manipulation_id 
				            AND ""EntityName"" = v_entity_name 
				            AND ""FieldName"" = 'ManMadeCauses';
				            
				            -- Insert ManMadeCauses
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'ManMadeCauses')
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
				                    'ManMadeCauses',
				                    v_habitat_manipulation_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        ELSE
				            -- Only delete if there are existing ManMadeCauses
				            IF EXISTS (
				                SELECT 1 FROM public.""EcologySelections"" 
				                WHERE ""EntityId"" = v_habitat_manipulation_id 
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'ManMadeCauses'
				            ) THEN
				                DELETE FROM public.""EcologySelections""
				                WHERE ""EntityId"" = v_habitat_manipulation_id
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'ManMadeCauses';
				            END IF;
				        END IF;	
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'EcologySelections (ManMadeCauses) table error: %', SQLERRM;
				    END;

					-- Update ManipulationDriver Data
				    BEGIN
				        IF input_data->'ManipulationDriver' IS NOT NULL AND jsonb_array_length(input_data->'ManipulationDriver') > 0 THEN
				        
				            -- Delete existing ManipulationDriver
				            DELETE FROM public.""EcologySelections""
				            WHERE ""EntityId"" = v_habitat_manipulation_id 
				            AND ""EntityName"" = v_entity_name 
				            AND ""FieldName"" = 'ManipulationDriver';
				            
				            -- Insert ManipulationDriver
				            FOR v_array_element IN 
				                SELECT jsonb_array_elements_text(input_data->'ManipulationDriver')
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
				                    'ManipulationDriver',
				                    v_habitat_manipulation_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        ELSE
				            -- Only delete if there are existing ManipulationDriver
				            IF EXISTS (
				                SELECT 1 FROM public.""EcologySelections"" 
				                WHERE ""EntityId"" = v_habitat_manipulation_id 
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'ManipulationDriver'
				            ) THEN
				                DELETE FROM public.""EcologySelections""
				                WHERE ""EntityId"" = v_habitat_manipulation_id
				                AND ""EntityName"" = v_entity_name
				                AND ""FieldName"" = 'ManipulationDriver';
				            END IF;
				        END IF;	
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'EcologySelections (ManipulationDriver) table error: %', SQLERRM;
				    END;
				    

				    -- Return the updated v_habitat_manipulation_id
				    RETURN v_habitat_manipulation_id::TEXT;
				    
				EXCEPTION
				    WHEN OTHERS THEN
				        RAISE EXCEPTION 'Error in fn_update_habitat_manipulation: %', SQLERRM;
				END;        
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_habitat_manipulations(integer, integer, character varying, character varying, character varying, uuid[]);
				DROP FUNCTION IF EXISTS public.fn_create_habitat_manipulation(jsonb);
				DROP FUNCTION IF EXISTS public.fn_habitat_manipulation_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_habitat_manipulation(jsonb, character varying);
            ");
        }
    }
}
