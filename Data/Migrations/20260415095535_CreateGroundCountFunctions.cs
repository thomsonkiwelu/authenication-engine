using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateGroundCountFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_ground_count(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_ground_count_id UUID;
						v_ground_count_sighting_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'GroundCountSighting';
						v_animal_observed_element JSONB;
						v_animal_count_element JSONB;
						v_park_id UUID;
						v_image_id TEXT;
					BEGIN 
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Insert GroundCounts data
						BEGIN
							INSERT INTO public.""GroundCounts""(
								""Id"",
								""ParkId"",
								""LocalAreaNameId"", 
								""TransectId"", 
								""Method"", 
								""TransectStartingPoint"", 
								""TransectEndPoint"", 
								""EndDistance"", 
								""RouteDescription"", 
								""WeatherCondition"", 
								""OtherWeatherCondition"",	
								""CreatedBy"",
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								v_park_id,
								(input_data->>'LocalAreaNameId')::UUID,
								input_data->>'TransectId',
								input_data->>'Method',
								input_data->>'TransectStartingPoint',
								input_data->>'TransectEndPoint',
								(input_data->>'EndDistance')::NUMERIC,
								input_data->>'RouteDescription',
								input_data->>'WeatherCondition',
								NULLIF(COALESCE(input_data->>'OtherWeatherCondition', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_ground_count_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'GroundCounts table error: %', SQLERRM;
						END;

						-- Insert AnimalObserved Data
						IF input_data->'AnimalObserved' IS NOT NULL AND jsonb_array_length(input_data->'AnimalObserved') > 0 THEN
						    FOR v_animal_observed_element IN
						        SELECT * FROM jsonb_array_elements(input_data->'AnimalObserved')
						    LOOP
						        BEGIN
									INSERT INTO public.""GroundCountSightings""(
										""Id"",
										""SpeciesId"",
										""GroundCountId"",
										""TimeOfSighting"",
										""PerpendicularDistance"",
										""Distance"",
										""Remark"",
										""Coordinates"",
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_animal_observed_element->>'SpeciesId')::UUID,
										v_ground_count_id,
										v_animal_observed_element->>'TimeOfSighting',
										(v_animal_observed_element->>'PerpendicularDistance')::NUMERIC,
										(v_animal_observed_element->>'Distance')::NUMERIC,
										v_animal_observed_element->>'Remark',
										NULLIF(COALESCE(v_animal_observed_element->>'Coordinates', ''),''),
										(input_data->>'CreatedBy')::UUID,
										NOW()
									) RETURNING ""Id"" INTO v_ground_count_sighting_id;

									-- Insert AnimalCount Data
									IF v_animal_observed_element->'AnimalCount' IS NOT NULL AND jsonb_array_length(v_animal_observed_element->'AnimalCount') > 0 THEN
									    FOR v_animal_count_element IN
									        SELECT * FROM jsonb_array_elements(v_animal_observed_element->'AnimalCount')
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
													v_animal_count_element->>'Age',
													v_animal_count_element->>'Sex',
													(v_animal_count_element->>'Count')::INT,
													v_ground_count_sighting_id,
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
								
									-- Insert Files Data
						            BEGIN
						                v_image_id := v_animal_observed_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
						                        ""EntityId"" = v_ground_count_sighting_id,
						                        ""EntityName"" = v_entity_name,
						                        ""UpdatedBy"" = (input_data->>'CreatedBy')::UUID,
						                        ""UpdatedAt"" = NOW()
						                    WHERE ""Id"" = v_image_id::UUID;
						                END IF;
										EXCEPTION
								            WHEN OTHERS THEN
								                RAISE EXCEPTION 'Files table error: %', SQLERRM;
						            END;
									
						        EXCEPTION
						            WHEN OTHERS THEN
						                RAISE EXCEPTION 'GroundCountSightings table error: %', SQLERRM;
						        END;
						    END LOOP;
						END IF;
						
						-- Return the created v_ground_count_id
						RETURN v_ground_count_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_ground_count: %', SQLERRM;
					END;	
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_ground_count(
					input_data jsonb,
					ground_count_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_ground_count_id UUID;
						v_ground_count_sighting_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'GroundCountSighting';
						v_animal_observed_element JSONB;
						v_animal_count_element JSONB;
						v_park_id UUID;
						v_image_id TEXT;
					BEGIN 
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Update GroundCounts data
						BEGIN
							-- Convert ground_count_id parameter to UUID
							v_ground_count_id := ground_count_id::UUID;
							
							UPDATE public.""GroundCounts""
							SET
								""ParkId"" = v_park_id,
								""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
								""TransectId"" = input_data->>'TransectId',
								""Method"" = input_data->>'Method',
								""TransectStartingPoint"" = input_data->>'TransectStartingPoint',
								""TransectEndPoint"" = input_data->>'TransectEndPoint',
								""EndDistance"" = (input_data->>'EndDistance')::NUMERIC,
								""RouteDescription"" = input_data->>'RouteDescription',
								""WeatherCondition"" = input_data->>'WeatherCondition',
								""OtherWeatherCondition"" = NULLIF(COALESCE(input_data->>'OtherWeatherCondition', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_ground_count_id
							RETURNING ""Id"" INTO v_ground_count_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'GroundCounts table error: %', SQLERRM;
						END;

						-- Update AnimalObserved Data
						BEGIN
							IF input_data->'AnimalObserved' IS NOT NULL AND jsonb_array_length(input_data->'AnimalObserved') > 0 THEN
								
								-- Delete existing GroundCountSightings
								DELETE FROM public.""GroundCountSightings""
								WHERE ""GroundCountId"" = v_ground_count_id;
							
								-- Insert GroundCountSightings data
								FOR v_animal_observed_element IN
									SELECT * FROM jsonb_array_elements(input_data->'AnimalObserved')
								LOOP
									INSERT INTO public.""GroundCountSightings""(
										""Id"",
										""SpeciesId"",
										""GroundCountId"",
										""TimeOfSighting"",
										""PerpendicularDistance"",
										""Distance"",
										""Remark"",
										""Coordinates"",
										""CreatedBy"",
										""CreatedAt"",
										""UpdatedBy"",
								        ""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_animal_observed_element->>'SpeciesId')::UUID,
										v_ground_count_id,
										v_animal_observed_element->>'TimeOfSighting',
										(v_animal_observed_element->>'PerpendicularDistance')::NUMERIC,
										(v_animal_observed_element->>'Distance')::NUMERIC,
										v_animal_observed_element->>'Remark',
										NULLIF(COALESCE(v_animal_observed_element->>'Coordinates', ''),''),
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									) RETURNING ""Id"" INTO v_ground_count_sighting_id;

									-- Insert AnimalCount Data
									IF v_animal_observed_element->'AnimalCount' IS NOT NULL AND jsonb_array_length(v_animal_observed_element->'AnimalCount') > 0 THEN

										-- Delete existing AnimalDemographics
										DELETE FROM public.""AnimalDemographics""
										WHERE ""EntityId"" = (v_animal_observed_element->>'Id')::UUID;
								
									    FOR v_animal_count_element IN
									        SELECT * FROM jsonb_array_elements(v_animal_observed_element->'AnimalCount')
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
													""CreatedAt"",
													""UpdatedBy"",
				        							""UpdatedAt""
												) VALUES (
													gen_random_uuid(),
													v_animal_count_element->>'Age',
													v_animal_count_element->>'Sex',
													(v_animal_count_element->>'Count')::INT,
													v_ground_count_sighting_id,
													v_entity_name,
													(input_data->>'UpdatedBy')::UUID,
													NOW(),
													(input_data->>'UpdatedBy')::UUID,
													NOW()
												);
											EXCEPTION
									            WHEN OTHERS THEN
									                RAISE EXCEPTION 'AnimalDemographics table error: %', SQLERRM;
									        END;
									    END LOOP;
									END IF;
									
									-- Insert Files Data
						            BEGIN
						                v_image_id := v_animal_observed_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
												""EntityId"" = v_ground_count_sighting_id,
						                        ""EntityName"" = v_entity_name,
						                        ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
						                        ""UpdatedAt"" = NOW()
						                    WHERE ""Id"" = v_image_id::UUID;
						                END IF;
										EXCEPTION
								            WHEN OTHERS THEN
								                RAISE EXCEPTION 'Files table error: %', SQLERRM;
						            END;
									
								END LOOP;
							END IF;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'GroundCountSightings table error: %', SQLERRM;
						END;

						-- Return the updated v_ground_count_id
						RETURN v_ground_count_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_ground_count: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_ground_count_by_id(
					ground_count_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', gc.""Id"",
								'ParkId', gc.""ParkId"",
								'LocalAreaNameId', gc.""LocalAreaNameId"",
								'TransectId', gc.""TransectId"",
								'Method', gc.""Method"",
								'TransectStartingPoint', gc.""TransectStartingPoint"",
								'TransectEndPoint', gc.""TransectEndPoint"",
								'EndDistance', gc.""EndDistance"",
								'RouteDescription', gc.""RouteDescription"",
								'WeatherCondition', gc.""WeatherCondition"",
								'OtherWeatherCondition', gc.""OtherWeatherCondition"",				
								'CreatedBy', gc.""CreatedBy"",
								'CreatedAt', gc.""CreatedAt"",
								'UpdatedBy', gc.""UpdatedBy"",
								'UpdatedAt', gc.""UpdatedAt"",
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
								'GroundCountSightings', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', gcs.""Id"",
											'SpeciesId', gcs.""SpeciesId"",
											'GroundCountId', gcs.""GroundCountId"",
											'TimeOfSighting', gcs.""TimeOfSighting"",
											'PerpendicularDistance', gcs.""PerpendicularDistance"",
											'Distance', gcs.""Distance"",
											'Coordinates', gcs.""Coordinates"",
											'Remark', gcs.""Remark"",	
											'CreatedAt', gcs.""CreatedAt"",
											'CreatedBy', gcs.""CreatedBy"",
											'UpdatedAt', gcs.""UpdatedAt"",
											'UpdatedBy', gcs.""UpdatedBy"",
											'Species', jsonb_build_object(
												'Id', s.""Id"",
												'CommonName', s.""CommonName"",
												'ScientificName', s.""ScientificName"",
												'Type', s.""Type"",
												'CreatedAt', s.""CreatedAt"",
												'CreatedBy', s.""CreatedBy""
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
												WHERE ad.""EntityId"" = gcs.""Id""
											)
										)
									), '[]'::JSONB)
									FROM ""GroundCountSightings"" gcs
									JOIN ""Species"" s ON s.""Id"" = gcs.""SpeciesId""
									WHERE gcs.""GroundCountId"" = gc.""Id""
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
									WHERE u.""Id"" = gc.""UpdatedBy""
								)
							),
							'{}'::JSONB
						) INTO response_data
						FROM ""GroundCounts"" gc
						JOIN ""Locations"" loc ON loc.""Id"" = gc.""LocalAreaNameId""
						JOIN ""Users"" u ON u.""Id"" = gc.""CreatedBy""
						WHERE gc.""Id"" = ground_count_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::JSONB);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_ground_count_by_id: %', SQLERRM;
					END;				
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_create_ground_count(jsonb);
				DROP FUNCTION IF EXISTS public.fn_update_ground_count(jsonb, character varying);
				DROP FUNCTION IF EXISTS public.fn_ground_count_by_id(uuid);
            ");
        }
    }
}
