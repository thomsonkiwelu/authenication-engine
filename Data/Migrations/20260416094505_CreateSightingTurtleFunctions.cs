using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateSightingTurtleFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_sighting_turtles(
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
						
						-- sighting_turtle_data
						WITH sighting_turtle_data AS (
							SELECT 
								st.""Id"",
								st.""LocalAreaNameId"",
								st.""ParkId"",
								st.""Coordinates"",
								st.""CreatedAt"",
								st.""CreatedBy"",
								(
									SELECT COALESCE(SUM(ad.""Count""), 0)
									FROM ""AnimalDemographics"" ad
									WHERE ad.""EntityId"" = st.""Id"" AND ad.""AgeCategory"" = 'adults'
								) AS ""TotalAdult"",
								(
									SELECT COALESCE(SUM(ad.""Count""), 0)
									FROM ""AnimalDemographics"" ad
									WHERE ad.""EntityId"" = st.""Id"" AND ad.""AgeCategory"" = 'hatchlings'
								) AS ""TotalHatchling"",
								loc.""Id"" AS location_id,
								loc.""Name"" AS location_name,
								p.""Id"" AS parkId,
								p.""Name"" AS parkName,
								u.""Id"" AS user_id,
								u.""Username"" AS username,
								ROW_NUMBER() OVER (ORDER BY st.""CreatedAt"" DESC) AS row_number,
								-- Get total count
								COUNT(*) OVER() AS full_count
							FROM ""SightingTurtles"" st
							JOIN ""Locations"" loc ON loc.""Id"" = st.""LocalAreaNameId""
							JOIN ""Parks"" p ON p.""Id"" = st.""ParkId""
							JOIN ""Users"" u ON u.""Id"" = st.""CreatedBy""
							WHERE
								(search_text IS NULL OR search_text = '' OR
								loc.""Name"" ILIKE '%' || search_text || '%')
							AND st.""DeletedAt"" IS NULL
							AND (park_id IS NULL OR park_id = '' OR st.""ParkId"" = park_id::UUID)
							AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR
								st.""ParkId"" = ANY(park_ids))
						),
						paginated_data AS (
							SELECT * FROM sighting_turtle_data
							WHERE row_number BETWEEN min_row_num AND max_row_num
						)
						SELECT 
							jsonb_agg(
								jsonb_build_object(
									'RowNumber', row_number,
									'Id', ""Id"",
									'LocalAreaNameId', ""LocalAreaNameId"",
									'ParkId', ""ParkId"",
									'Coordinates', ""Coordinates"",
									'TotalAdult', ""TotalAdult"",
									'TotalHatchling', ""TotalHatchling"",
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
							RAISE EXCEPTION 'Error in fn_sighting_turtles: %', SQLERRM;
					END;				
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_sighting_turtle(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_sighting_turtle_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'SightingTurtle';
						v_animal_demographic_element JSONB;
						v_park_id UUID;
					BEGIN  
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Insert SightingTurtles data
						BEGIN
							INSERT INTO public.""SightingTurtles""(
								""Id"", 
								""LocalAreaNameId"",
								""ParkId"",	
								""Coordinates"",
								""Remark"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'LocalAreaNameId')::UUID,
								v_park_id,
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								NULLIF(COALESCE(input_data->>'Remark', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_sighting_turtle_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'SightingTurtles table error: %', SQLERRM;
						END;

						-- Insert AnimalDemographics Data
						IF input_data->'AnimalDemographics' IS NOT NULL AND jsonb_array_length(input_data->'AnimalDemographics') > 0 THEN
						    FOR v_animal_demographic_element IN
						        SELECT * FROM jsonb_array_elements(input_data->'AnimalDemographics')
						    LOOP
						        BEGIN
						            -- Insert Male record if count > 0
						            IF (v_animal_demographic_element->>'NumberOfMale') IS NOT NULL AND (v_animal_demographic_element->>'NumberOfMale')::INT > 0 THEN
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
						                    v_animal_demographic_element->>'Category',
						                    'male',
						                    (v_animal_demographic_element->>'NumberOfMale')::INT,
						                    v_sighting_turtle_id,
						                    v_entity_name,
						                    (input_data->>'CreatedBy')::UUID,
						                    NOW()
						                );
						            END IF;
						            
						            -- Insert Female record if count > 0
						            IF (v_animal_demographic_element->>'NumberOfFemale') IS NOT NULL AND (v_animal_demographic_element->>'NumberOfFemale')::INT > 0 THEN
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
						                    v_animal_demographic_element->>'Category',
						                    'female',
						                    (v_animal_demographic_element->>'NumberOfFemale')::INT,
						                    v_sighting_turtle_id,
						                    v_entity_name,
						                    (input_data->>'CreatedBy')::UUID,
						                    NOW()
						                );
						            END IF;
						            
						        EXCEPTION
						            WHEN OTHERS THEN
						                RAISE EXCEPTION 'AnimalDemographics table error: %', SQLERRM;
						        END;
						    END LOOP;
						END IF;
						
						-- Return the created v_sighting_turtle_id
						RETURN v_sighting_turtle_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_sighting_turtle: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_sighting_turtle(
					input_data jsonb,
					sighting_turtle_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_sighting_turtle_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'SightingTurtle';
						v_animal_demographic_element JSONB;
						v_park_id UUID;
					BEGIN 
						-- Update SightingTurtle data
						BEGIN
							-- Convert v_sighting_turtle_id parameter to UUID
							v_sighting_turtle_id := sighting_turtle_id::UUID;

							-- Get ParkId
							SELECT ""ParkId"" INTO v_park_id
							FROM ""Locations""
							WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
							
							UPDATE public.""SightingTurtles""
							SET
								""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
								""ParkId"" = v_park_id,
								""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								""Remark"" = NULLIF(COALESCE(input_data->>'Remark', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_sighting_turtle_id
							RETURNING ""Id"" INTO v_sighting_turtle_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'SightingTurtles table error: %', SQLERRM;
						END;

						-- Insert AnimalDemographics Data
						IF input_data->'AnimalDemographics' IS NOT NULL AND jsonb_array_length(input_data->'AnimalDemographics') > 0 THEN
						    
							-- Delete existing AnimalDemographics
							DELETE FROM public.""AnimalDemographics""
							WHERE ""EntityId"" = v_sighting_turtle_id;
							
							FOR v_animal_demographic_element IN
						        SELECT * FROM jsonb_array_elements(input_data->'AnimalDemographics')
						    LOOP
						        BEGIN
						            -- Insert Male record if count > 0
						            IF (v_animal_demographic_element->>'NumberOfMale') IS NOT NULL AND (v_animal_demographic_element->>'NumberOfMale')::INT > 0 THEN
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
						                    v_animal_demographic_element->>'Category',
						                    'male',
						                    (v_animal_demographic_element->>'NumberOfMale')::INT,
						                    v_sighting_turtle_id,
						                    v_entity_name,
						                    (input_data->>'UpdatedBy')::UUID,
						                    NOW(),
											(input_data->>'UpdatedBy')::UUID,
						                    NOW()
						                );
						            END IF;
						            
						            -- Insert Female record if count > 0
						            IF (v_animal_demographic_element->>'NumberOfFemale') IS NOT NULL AND (v_animal_demographic_element->>'NumberOfFemale')::INT > 0 THEN
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
						                    v_animal_demographic_element->>'Category',
						                    'female',
						                    (v_animal_demographic_element->>'NumberOfFemale')::INT,
						                    v_sighting_turtle_id,
						                    v_entity_name,
						                    (input_data->>'UpdatedBy')::UUID,
						                    NOW(),
											(input_data->>'UpdatedBy')::UUID,
						                    NOW()
						                );
						            END IF;
						            
						        EXCEPTION
						            WHEN OTHERS THEN
						                RAISE EXCEPTION 'AnimalDemographics table error: %', SQLERRM;
						        END;
						    END LOOP;
						END IF;
						
						-- Return the updated v_sighting_turtle_id
						RETURN v_sighting_turtle_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_sighting_turtle: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_sighting_turtle_by_id(
					sighting_turtle_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_entity_name CONSTANT VARCHAR(50) := 'SightingTurtle';
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', st.""Id"",
								'LocalAreaNameId', st.""LocalAreaNameId"",
								'ParkId', st.""ParkId"",
								'Coordinates', st.""Coordinates"",
								'Remark', st.""Remark"",
								'CreatedBy', st.""CreatedBy"",
								'CreatedAt', st.""CreatedAt"",
								'UpdatedBy', st.""UpdatedBy"",
								'UpdatedAt', st.""UpdatedAt"",
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
								'TotalHatchling', (
									SELECT COALESCE(SUM(ad.""Count""), 0)
									FROM ""AnimalDemographics"" ad
									WHERE ad.""EntityId"" = st.""Id"" AND ad.""AgeCategory"" = 'hatchlings'
								),
								'TotalAdult', (
									SELECT COALESCE(SUM(ad.""Count""), 0)
									FROM ""AnimalDemographics"" ad
									WHERE ad.""EntityId"" = st.""Id"" AND ad.""AgeCategory"" = 'adults'
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
									WHERE ad.""EntityId"" = st.""Id""
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
									WHERE u.""Id"" = st.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""SightingTurtles"" st
						JOIN ""Locations"" loc ON loc.""Id"" = st.""LocalAreaNameId""
						JOIN ""Parks"" p ON p.""Id"" = st.""ParkId""
						JOIN ""Users"" u ON u.""Id"" = st.""CreatedBy""
						WHERE st.""Id"" = sighting_turtle_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_sighting_turtle_by_id: %', SQLERRM;
					END;		
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_sighting_turtles(integer, integer, character varying, character varying, uuid[]);
				DROP FUNCTION IF EXISTS public.fn_create_sighting_turtle(jsonb);
				DROP FUNCTION IF EXISTS public.fn_update_sighting_turtle(jsonb, character varying);
				DROP FUNCTION IF EXISTS public.fn_sighting_turtle_by_id(uuid);
            ");
        }
    }
}
