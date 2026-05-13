using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateRoadKillFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_road_kills(
					page_number integer,
					page_size integer,
					search_text character varying DEFAULT ''::character varying,
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
						
						-- road_kill_data
						WITH road_kill_data AS (
							SELECT 
								rok.""Id"",
								rok.""LocalAreaNameId"",
								rok.""CreatedAt"",
								rok.""CreatedBy"",
								(
			    					SELECT COALESCE(SUM(ad.""Count""), 0)
							        FROM ""AnimalDemographics"" ad
							        WHERE ad.""EntityId"" = rok.""Id""
							    ) AS ""TotalAnimalCount"",
								loc.""Id"" AS location_id,
								loc.""Name"" AS location_name,
								p.""Id"" AS parkId,
								p.""Name"" AS parkName,
								sp.""Id"" AS species_id,
								sp.""CommonName"" AS species_common_name,
								sp.""ScientificName"" AS species_scientific_name,
								u.""Id"" AS user_id,
								u.""Username"" AS username,
								ROW_NUMBER() OVER (ORDER BY rok.""CreatedAt"" DESC) AS row_number,
								-- Get total count
								COUNT(*) OVER() AS full_count
							FROM ""RoadKills"" rok
							JOIN ""Locations"" loc ON loc.""Id"" = rok.""LocalAreaNameId""
							JOIN ""Parks"" p ON p.""Id"" = rok.""ParkId""
							JOIN ""Species"" sp ON sp.""Id"" = rok.""SpeciesId""
							JOIN ""Users"" u ON u.""Id"" = rok.""CreatedBy""
							WHERE
								(search_text IS NULL OR search_text = '' OR
								loc.""Name"" ILIKE '%' || search_text || '%' OR
								sp.""ScientificName"" ILIKE '%' || search_text || '%' OR
								sp.""CommonName"" ILIKE '%' || search_text || '%')
							AND rok.""DeletedAt"" IS NULL
							AND (park_id IS NULL OR park_id = '' OR rok.""ParkId"" = park_id::UUID)
							AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR
     							rok.""ParkId"" = ANY(park_ids))
						),
						paginated_data AS (
							SELECT * FROM road_kill_data
							WHERE row_number BETWEEN min_row_num AND max_row_num
						)
						SELECT 
							jsonb_agg(
								jsonb_build_object(
									'RowNumber', row_number,
									'Id', ""Id"",
									'LocalAreaNameId', ""LocalAreaNameId"",
									'TotalAnimalCount', ""TotalAnimalCount"",
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
							RAISE EXCEPTION 'Error in fn_road_kills: %', SQLERRM;
					END;        
				$BODY$;
            ");

	        migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_road_kill(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_road_kill_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'RoadKill';
						v_animal_killed_element JSONB;
						v_park_id UUID;
						v_image_id TEXT;
					BEGIN  
						-- Get ParkId
						SELECT ""ParkId"" INTO v_park_id 
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Insert RoadKills data
						BEGIN
							INSERT INTO public.""RoadKills""(
								""Id"",
								""SpeciesId"",
								""LocalAreaNameId"",
								""ParkId"",
								""Coordinates"",
								""Remark"",
								""CreatedBy"",
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'SpeciesId')::UUID,
								(input_data->>'LocalAreaNameId')::UUID,
								v_park_id,
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								NULLIF(COALESCE(input_data->>'Remark', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_road_kill_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'RoadKills table error: %', SQLERRM;
						END;

						-- Insert Files Data
						BEGIN
							v_image_id := input_data->>'ImageId';
							IF v_image_id IS NOT NULL AND v_image_id != '' THEN
								UPDATE public.""Files""
								SET
									""EntityId"" = v_road_kill_id,
									""EntityName"" = v_entity_name,
									""UpdatedBy"" = (input_data->>'CreatedBy')::UUID,
									""UpdatedAt"" = NOW()
								WHERE ""Id"" = v_image_id::UUID;
							END IF;
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'Files table error: %', SQLERRM;
						END;

						-- Insert AnimalDemographics Data
						IF input_data->'AnimalKilled' IS NOT NULL AND jsonb_array_length(input_data->'AnimalKilled') > 0 THEN
							FOR v_animal_killed_element IN
								SELECT * FROM jsonb_array_elements(input_data->'AnimalKilled')
							LOOP
								BEGIN
									INSERT INTO public.""AnimalDemographics""(
										""Id"",
										""AgeCategory"",
										""Sex"", 
										""Count"",
										""EntityId"",
										""EntityName"",
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_animal_killed_element->>'Age',
										v_animal_killed_element->>'Sex',
										(v_animal_killed_element->>'Count')::INT,
										v_road_kill_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'AnimalDemographics table error: %', SQLERRM;
								END;
							END LOOP;
						END IF;
						
						-- Return the created v_road_kill_id
						RETURN v_road_kill_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_road_kill: %', SQLERRM;
					END;		
				$BODY$;
			");

	        migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_road_kill_by_id(
					road_kill_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', rok.""Id"",
								'LocalAreaNameId', rok.""LocalAreaNameId"",
								'SpeciesId', rok.""SpeciesId"",
								'Coordinates', rok.""Coordinates"",
								'Remark', rok.""Remark"",
								'TotalAnimalCount', (
			    					SELECT COALESCE(SUM(ad.""Count""), 0)
							        FROM ""AnimalDemographics"" ad
							        WHERE ad.""EntityId"" = rok.""Id""
							    ),
								'CreatedBy', rok.""CreatedBy"",
								'CreatedAt', rok.""CreatedAt"",
								'UpdatedBy', rok.""UpdatedBy"",
								'UpdatedAt', rok.""UpdatedAt"",
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
								'AnimalDemographics', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', ad.""Id"",
											'AgeCategory', ad.""AgeCategory"",
											'Sex', ad.""Sex"",
											'Count', ad.""Count"",
											'EntityId', ad.""EntityId"",
											'EntityName', ad.""EntityName"",
											'CreatedAt', ad.""CreatedAt"",
											'CreatedBy', ad.""CreatedBy"",
											'UpdatedAt', ad.""UpdatedAt"",
											'UpdatedBy', ad.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""AnimalDemographics"" ad
									WHERE ad.""EntityId"" = rok.""Id""
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
									WHERE u.""Id"" = rok.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""RoadKills"" rok
						JOIN ""Locations"" loc ON loc.""Id"" = rok.""LocalAreaNameId""
						JOIN ""Species"" sp ON sp.""Id"" = rok.""SpeciesId""
						JOIN ""Users"" u ON u.""Id"" = rok.""CreatedBy""
						WHERE rok.""Id"" = road_kill_id;
						
						-- data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_road_kill_by_id: %', SQLERRM;
					END;		
				$BODY$;
			");

	        migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_road_kill(
					input_data jsonb,
					road_kill_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE
				    v_road_kill_id UUID;
				    v_entity_name CONSTANT VARCHAR(50) := 'RoadKills';
				    v_animal_killed_element JSONB;
				    v_array_element TEXT;
					v_park_id UUID;
					v_image_id TEXT;
				BEGIN 
				    -- Update RoadKills data
				    BEGIN
				        -- Convert v_road_kill_id parameter to UUID
				        v_road_kill_id := road_kill_id::UUID;

						-- Get ParkId
						SELECT ""ParkId"" INTO v_park_id 
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
				        UPDATE public.""RoadKills""
				        SET
				            ""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
							""ParkId"" = v_park_id,
				            ""SpeciesId"" =  (input_data->>'SpeciesId')::UUID,
				            ""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
				            ""Remark"" = NULLIF(COALESCE(input_data->>'Remark', ''),''),
				            ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
				            ""UpdatedAt"" = NOW()
				        WHERE ""Id"" = v_road_kill_id
				        RETURNING ""Id"" INTO v_road_kill_id;
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'RoadKills table error: %', SQLERRM;
				    END;
					
					-- Insert Files Data
					BEGIN
						v_image_id := input_data->>'ImageId';
						IF v_image_id IS NOT NULL AND v_image_id != '' THEN
							UPDATE public.""Files""
							SET
								""EntityId"" = v_road_kill_id,
								""EntityName"" = v_entity_name,
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_image_id::UUID;
						END IF;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'Files table error: %', SQLERRM;
					END;

				    -- Update AnimalDemographics Data
				    BEGIN
				        IF input_data->'AnimalKilled' IS NOT NULL AND jsonb_array_length(input_data->'AnimalKilled') > 0 THEN
				            
				            -- Delete existing AnimalDemographics
				            DELETE FROM public.""AnimalDemographics""
				            WHERE ""EntityId"" = v_road_kill_id;
				                
				            -- Insert v_road_kill_id data	
				            FOR v_animal_killed_element IN
				                SELECT * FROM jsonb_array_elements(input_data->'AnimalKilled')
				            LOOP
				                INSERT INTO public.""AnimalDemographics""(
				                    ""Id"",
									""AgeCategory"",
									""Sex"",
									""Count"",
				                    ""EntityId"",
				                    ""EntityName"",
				                    ""CreatedBy"",
				                    ""CreatedAt"",
				                    ""UpdatedBy"",
				                    ""UpdatedAt""
				                ) VALUES (
				                    gen_random_uuid(),
									v_animal_killed_element->>'Age',
									v_animal_killed_element->>'Sex',
									(v_animal_killed_element->>'Count')::INT,
				                    v_road_kill_id,
				                    v_entity_name,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
				        END IF;
					    EXCEPTION
					        WHEN OTHERS THEN
					            RAISE EXCEPTION 'AnimalDemographics table error: %', SQLERRM;
				    END;

				    -- Return the updated v_road_kill_id
				    RETURN v_road_kill_id::TEXT;
				    
				EXCEPTION
				    WHEN OTHERS THEN
				        RAISE EXCEPTION 'Error in fn_update_road_kill: %', SQLERRM;
				END;        
				$BODY$;
			");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS public.fn_road_kills(integer, integer, character varying, character varying, uuid[]);
				DROP FUNCTION IF EXISTS public.fn_create_road_kill(jsonb);
				DROP FUNCTION IF EXISTS public.fn_road_kill_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_road_kill(jsonb, character varying);
            ");
        }
    }
}
