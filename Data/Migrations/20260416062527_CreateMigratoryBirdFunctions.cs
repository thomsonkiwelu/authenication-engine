using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMigratoryBirdFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_line_transects(
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
						
						-- line_transect_data
						WITH line_transect_data AS (
							SELECT 
								lt.""Id"",
								lt.""ParkId"",
								lt.""AlongTransectLocalAreaNameId"",
								lt.""LineTransectLocalAreaNameId"",
								lt.""CreatedAt"",
								lt.""CreatedBy"",
								(
									SELECT COALESCE(SUM(ad.""IndividualObserved""), 0)
									FROM ""MigratoryBirds"" ad
									WHERE ad.""EntityId"" = lt.""Id""
								) AS ""TotalMigratoryBirdCount"",
								lineloc.""Id"" AS line_location_id,
								lineloc.""Name"" AS line_location_name,
								alongloc.""Id"" AS along_location_id,
								alongloc.""Name"" AS along_location_name,
								p.""Id"" AS parkId,
								p.""Name"" AS parkName,
								u.""Id"" AS user_id,
								u.""Username"" AS username,
								ROW_NUMBER() OVER (ORDER BY lt.""CreatedAt"" DESC) AS row_number,
								-- Get total count
								COUNT(*) OVER() AS full_count
							FROM ""LineTransects"" lt
							JOIN ""Locations"" lineloc ON lineloc.""Id"" = lt.""LineTransectLocalAreaNameId""
							JOIN ""Parks"" p ON p.""Id"" = lt.""ParkId""
							JOIN ""Locations"" alongloc ON alongloc.""Id"" = lt.""AlongTransectLocalAreaNameId""
							JOIN ""Users"" u ON u.""Id"" = lt.""CreatedBy""
							WHERE
								(search_text IS NULL OR search_text = '' OR
								p.""Name"" ILIKE '%' || search_text || '%')
							AND lt.""DeletedAt"" IS NULL
							AND (park_id IS NULL OR park_id = '' OR lt.""ParkId"" = park_id::UUID)
							AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR
								lt.""ParkId"" = ANY(park_ids))
						),
						paginated_data AS (
							SELECT * FROM line_transect_data
							WHERE row_number BETWEEN min_row_num AND max_row_num
						)
						SELECT 
							jsonb_agg(
								jsonb_build_object(
									'RowNumber', row_number,
									'Id', ""Id"",
									'ParkId', ""ParkId"",
									'AlongTransectLocalAreaNameId', ""AlongTransectLocalAreaNameId"",
									'LineTransectLocalAreaNameId', ""LineTransectLocalAreaNameId"",
									'TotalMigratoryBirdCount', ""TotalMigratoryBirdCount"",
									'CreatedAt', ""CreatedAt"",
									'CreatedBy', ""CreatedBy"",
									'LineTransectLocation', jsonb_build_object(
										'Id', line_location_id,
										'Name', line_location_name
									),
									'AlongTransectLocation', jsonb_build_object(
										'Id', along_location_id,
										'Name', along_location_name
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
							RAISE EXCEPTION 'Error in fn_line_transects: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_line_transect(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_line_transect_id UUID;
						v_migratory_bird_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'LineTransect';
						v_migratory_bird_entity CONSTANT VARCHAR(50) := 'MigratoryBird';
						v_line_transect_element JSONB;
						v_along_transect_element JSONB;
					BEGIN  
						-- Insert LineTransects data
						BEGIN
							INSERT INTO public.""LineTransects""(
								""Id"",
								""ParkId"",
								""Session"",
								""LineTransectStartCoordinates"",
								""LineTransectRecordAltitude"",
								""LineTransectHabitat"",
								""LineTransectLocalAreaNameId"",
								""AlongTransectCoordinates"",
								""AlongTransectRecordAltitude"",
								""AlongTransectHabitat"", 
								""AlongTransectLocalAreaNameId"",
								""EndTransectCoordinates"", 
								""EndTransectRecordAltitude"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'ParkId')::UUID,
								input_data->>'Session',
								NULLIF(COALESCE(input_data->>'LineTransectStartCoordinates', ''),''),
								NULLIF(COALESCE(input_data->>'LineTransectRecordAltitude', ''),''),
								input_data->>'LineTransectHabitat',
								(input_data->>'LineTransectLocalAreaNameId')::UUID,
								NULLIF(COALESCE(input_data->>'AlongTransectCoordinates', ''),''),
								NULLIF(COALESCE(input_data->>'AlongTransectRecordAltitude', ''),''),
								input_data->>'AlongTransectHabitat',
								(input_data->>'AlongTransectLocalAreaNameId')::UUID,
								NULLIF(COALESCE(input_data->>'EndTransectCoordinates', ''),''),
								NULLIF(COALESCE(input_data->>'EndTransectRecordAltitude', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_line_transect_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'LineTransects table error: %', SQLERRM;
						END;

						-- Insert LineTransectSpeciesObserved Data
						IF input_data->'LineTransectSpeciesObserved' IS NOT NULL AND jsonb_array_length(input_data->'LineTransectSpeciesObserved') > 0 THEN
						    FOR v_line_transect_element IN
						        SELECT * FROM jsonb_array_elements(input_data->'LineTransectSpeciesObserved')
						    LOOP
						        BEGIN
									INSERT INTO public.""MigratoryBirds""(
										""Id"", 
										""SpeciesId"", 
										""MigrationType"", 
										""Arrival"", 
										""Activity"", 
										""IndividualObserved"",
										""Remark"", 
										""Category"", 
										""EntityId"", 
										""EntityName"",
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_line_transect_element->>'SpeciesId')::UUID,
										v_line_transect_element->>'MigrationType',
										v_line_transect_element->>'Arrival',
										v_line_transect_element->>'Activity',
										(v_line_transect_element->>'IndividualObserved')::INT,
										v_line_transect_element->>'Remark',
										'LineTransectStart',
										v_line_transect_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									) RETURNING ""Id"" INTO v_migratory_bird_id;
									
									-- Insert Files Data
									DECLARE v_image_id TEXT;
						            BEGIN
						                v_image_id := v_line_transect_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
						                        ""EntityId"" = v_migratory_bird_id,
						                        ""EntityName"" = v_migratory_bird_entity,
						                        ""UpdatedBy"" = (input_data->>'CreatedBy')::UUID,
						                        ""UpdatedAt"" = NOW()
						                    WHERE ""Id"" = v_image_id::UUID;
						                END IF;
										EXCEPTION
								            WHEN OTHERS THEN
								                RAISE EXCEPTION 'Files (LineTransectSpeciesObserved) table error: %', SQLERRM;
						            END;
									
						        EXCEPTION
						            WHEN OTHERS THEN
						                RAISE EXCEPTION 'MigratoryBirds (LineTransectSpeciesObserved) table error: %', SQLERRM;
						        END;
						    END LOOP;
						END IF;

						-- Insert AlongTransectSpeciesObserved Data
						IF input_data->'AlongTransectSpeciesObserved' IS NOT NULL AND jsonb_array_length(input_data->'AlongTransectSpeciesObserved') > 0 THEN
						    FOR v_along_transect_element IN
						        SELECT * FROM jsonb_array_elements(input_data->'AlongTransectSpeciesObserved')
						    LOOP
						        BEGIN
									INSERT INTO public.""MigratoryBirds""(
										""Id"",
										""SpeciesId"",
										""MigrationType"",
										""Arrival"",
										""Activity"",
										""IndividualObserved"",
										""Remark"",
										""Category"",
										""EntityId"",
										""EntityName"",
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_along_transect_element->>'SpeciesId')::UUID,
										v_along_transect_element->>'MigrationType',
										v_along_transect_element->>'Arrival',
										v_along_transect_element->>'Activity',
										(v_along_transect_element->>'IndividualObserved')::INT,
										v_along_transect_element->>'Remark',
										'AlongTransect',
										v_line_transect_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									) RETURNING ""Id"" INTO v_migratory_bird_id;
					
									-- Insert Files Data
									DECLARE v_image_id TEXT;
						            BEGIN
						                v_image_id := v_along_transect_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
						                        ""EntityId"" = v_migratory_bird_id,
						                        ""EntityName"" = v_migratory_bird_entity,
						                        ""UpdatedBy"" = (input_data->>'CreatedBy')::UUID,
						                        ""UpdatedAt"" = NOW()
						                    WHERE ""Id"" = v_image_id::UUID;
						                END IF;
										EXCEPTION
								            WHEN OTHERS THEN
								                RAISE EXCEPTION 'Files (AlongTransectSpeciesObserved) table error: %', SQLERRM;
						            END;
									
						        EXCEPTION
						            WHEN OTHERS THEN
						                RAISE EXCEPTION 'MigratoryBirds (AlongTransectSpeciesObserved) table error: %', SQLERRM;
						        END;
						    END LOOP;
						END IF;
						
						-- Return the created v_line_transect_id
						RETURN v_line_transect_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_line_transect: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_line_transect(
					input_data jsonb,
					line_transect_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_line_transect_id UUID;
						v_migratory_bird_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'LineTransect';
						v_migratory_bird_entity CONSTANT VARCHAR(50) := 'MigratoryBird';
						v_line_transect_element JSONB;
						v_along_transect_element JSONB;
					BEGIN 
						-- Update LineTransects data
						BEGIN
							-- Convert v_line_transect_id parameter to UUID
							v_line_transect_id := line_transect_id::UUID;
							
							UPDATE public.""LineTransects""
							SET
								""ParkId"" = (input_data->>'ParkId')::UUID,
								""Session"" = input_data->>'Session',
								""LineTransectStartCoordinates"" = NULLIF(COALESCE(input_data->>'LineTransectStartCoordinates', ''),''),
								""LineTransectRecordAltitude"" = NULLIF(COALESCE(input_data->>'LineTransectRecordAltitude', ''),''),
								""LineTransectHabitat"" = input_data->>'LineTransectHabitat',
								""LineTransectLocalAreaNameId"" = (input_data->>'LineTransectLocalAreaNameId')::UUID,
								""AlongTransectCoordinates"" = NULLIF(COALESCE(input_data->>'AlongTransectCoordinates', ''),''),
								""AlongTransectRecordAltitude"" = NULLIF(COALESCE(input_data->>'AlongTransectRecordAltitude', ''),''),
								""AlongTransectHabitat"" = input_data->>'AlongTransectHabitat',
								""AlongTransectLocalAreaNameId"" = (input_data->>'AlongTransectLocalAreaNameId')::UUID,
								""EndTransectCoordinates"" = NULLIF(COALESCE(input_data->>'EndTransectCoordinates', ''),''),
								""EndTransectRecordAltitude"" = NULLIF(COALESCE(input_data->>'EndTransectRecordAltitude', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_line_transect_id
							RETURNING ""Id"" INTO v_line_transect_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'LineTransects table error: %', SQLERRM;
						END;

						-- Update MigratoryBirds(LineTransectSpeciesObserved) Data
						BEGIN
							IF input_data->'LineTransectSpeciesObserved' IS NOT NULL AND jsonb_array_length(input_data->'LineTransectSpeciesObserved') > 0 THEN
								
								-- Delete existing MigratoryBirds
								DELETE FROM public.""MigratoryBirds""
								WHERE ""EntityId"" = v_line_transect_id
								AND ""Category"" = 'LineTransectStart';
								
								-- Insert MigratoryBirds data
								FOR v_line_transect_element IN
									SELECT * FROM jsonb_array_elements(input_data->'LineTransectSpeciesObserved')
								LOOP
									INSERT INTO public.""MigratoryBirds""(
										""Id"", 
										""SpeciesId"", 
										""MigrationType"", 
										""Arrival"", 
										""Activity"", 
										""IndividualObserved"", 
										""Remark"", 
										""Category"", 
										""EntityId"", 
										""EntityName"",
										""CreatedBy"",
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_line_transect_element->>'SpeciesId')::UUID,
										v_line_transect_element->>'MigrationType',
										v_line_transect_element->>'Arrival',
										v_line_transect_element->>'Activity',
										(v_line_transect_element->>'IndividualObserved')::INT,
										v_line_transect_element->>'Remark',
										'LineTransectStart',
										v_line_transect_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									)RETURNING ""Id"" INTO v_migratory_bird_id;
									
									-- Insert Files Data
									DECLARE v_image_id TEXT;
						            BEGIN
						                v_image_id := v_line_transect_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
						                        ""EntityId"" = v_migratory_bird_id,
						                        ""EntityName"" = v_migratory_bird_entity,
						                        ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
						                        ""UpdatedAt"" = NOW()
						                    WHERE ""Id"" = v_image_id::UUID;
						                END IF;
										EXCEPTION
								            WHEN OTHERS THEN
								                RAISE EXCEPTION 'Files (LineTransectSpeciesObserved) table error: %', SQLERRM;
						            END;
									
								END LOOP;
							END IF;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'MigratoryBirds (LineTransectSpeciesObserved) table error: %', SQLERRM;
						END;

						-- Update MigratoryBirds(AlongTransectSpeciesObserved) Data
						BEGIN
							IF input_data->'AlongTransectSpeciesObserved' IS NOT NULL AND jsonb_array_length(input_data->'AlongTransectSpeciesObserved') > 0 THEN
								
								-- Delete existing MigratoryBirds
								DELETE FROM public.""MigratoryBirds""
								WHERE ""EntityId"" = v_line_transect_id
								AND ""Category"" = 'AlongTransect';
								
								-- Insert MigratoryBirds data
								FOR v_along_transect_element IN
									SELECT * FROM jsonb_array_elements(input_data->'AlongTransectSpeciesObserved')
								LOOP
									INSERT INTO public.""MigratoryBirds""(
										""Id"", 
										""SpeciesId"", 
										""MigrationType"", 
										""Arrival"", 
										""Activity"", 
										""IndividualObserved"", 
										""Remark"", 
										""Category"", 
										""EntityId"", 
										""EntityName"",
										""CreatedBy"",
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_along_transect_element->>'SpeciesId')::UUID,
										v_along_transect_element->>'MigrationType',
										v_along_transect_element->>'Arrival',
										v_along_transect_element->>'Activity',
										(v_along_transect_element->>'IndividualObserved')::INT,
										v_along_transect_element->>'Remark',
										'AlongTransect',
										v_line_transect_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									) RETURNING ""Id"" INTO v_migratory_bird_id;
									
									-- Insert Files Data
									DECLARE v_image_id TEXT;
						            BEGIN
						                v_image_id := v_along_transect_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
						                        ""EntityId"" = v_migratory_bird_id,
						                        ""EntityName"" = v_migratory_bird_entity,
						                        ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
						                        ""UpdatedAt"" = NOW()
						                    WHERE ""Id"" = v_image_id::UUID;
						                END IF;
										EXCEPTION
								            WHEN OTHERS THEN
								                RAISE EXCEPTION 'Files (AlongTransectSpeciesObserved) table error: %', SQLERRM;
						            END;
									
								END LOOP;
							END IF;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'MigratoryBirds(AlongTransectSpeciesObserved) table error: %', SQLERRM;
						END;

						-- Return the updated v_line_transect_id
						RETURN v_line_transect_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_line_transect: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_line_transect_by_id(
					line_transect_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', lt.""Id"",
								'Session', lt.""Session"",
								'ParkId', lt.""ParkId"",
								'LineTransectRecordAltitude', lt.""LineTransectRecordAltitude"",
								'LineTransectStartCoordinates', lt.""LineTransectStartCoordinates"",
								'LineTransectHabitat', lt.""LineTransectHabitat"",
								'LineTransectLocalAreaNameId', lt.""LineTransectLocalAreaNameId"",
								'AlongTransectCoordinates', lt.""AlongTransectCoordinates"",
								'AlongTransectRecordAltitude', lt.""AlongTransectRecordAltitude"",
								'AlongTransectHabitat', lt.""AlongTransectHabitat"",
								'AlongTransectLocalAreaNameId', lt.""AlongTransectLocalAreaNameId"",	
								'EndTransectCoordinates', lt.""EndTransectCoordinates"",
								'EndTransectRecordAltitude', lt.""EndTransectRecordAltitude"",
								'CreatedBy', lt.""CreatedBy"",
								'CreatedAt', lt.""CreatedAt"",
								'UpdatedBy', lt.""UpdatedBy"",
								'UpdatedAt', lt.""UpdatedAt"",
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'LineTransectLocation', jsonb_build_object(
									'Id', ltl.""Id"",
									'Name', ltl.""Name"",
									'ParkId', ltl.""ParkId"",
									'CreatedBy', ltl.""CreatedBy"",
									'CreatedAt', ltl.""CreatedAt""
								),
								'AlongTransectLocation', jsonb_build_object(
									'Id', alt.""Id"",
									'Name', alt.""Name"",
									'ParkId', alt.""ParkId"",
									'CreatedBy', alt.""CreatedBy"",
									'CreatedAt', alt.""CreatedAt""
								),
								'AlongTransectMigratoryBirds', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', m.""Id"",
											'SpeciesId', m.""SpeciesId"",
											'MigrationType', m.""MigrationType"",
											'Arrival', m.""Arrival"",
											'Activity', m.""Activity"",
											'IndividualObserved', m.""IndividualObserved"",
											'Remark', m.""Remark"",
											'Category', m.""Category"",						
											'EntityId', m.""EntityId"",
											'EntityName', m.""EntityName"",
											'CreatedAt', m.""CreatedAt"",
											'CreatedBy', m.""CreatedBy"",
											'UpdatedAt', m.""UpdatedAt"",
											'UpdatedBy', m.""UpdatedBy"",
											'Species', jsonb_build_object(
												'Id', s.""Id"",
												'CommonName', s.""CommonName"",
												'ScientificName', s.""ScientificName"",
												'Type', s.""Type"",
												'CreatedAt', s.""CreatedAt"",
												'CreatedBy', s.""CreatedBy""
											)
										)
									), '[]'::JSONB)
									FROM ""MigratoryBirds"" m
									JOIN ""Species"" s ON s.""Id"" = m.""SpeciesId""
									WHERE m.""EntityId"" = lt.""Id""
									AND m.""Category"" = 'AlongTransect'
								),
								'LineTransectMigratoryBirds', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', m.""Id"",
											'SpeciesId', m.""SpeciesId"",
											'MigrationType', m.""MigrationType"",
											'Arrival', m.""Arrival"",
											'Activity', m.""Activity"",
											'IndividualObserved', m.""IndividualObserved"",
											'Remark', m.""Remark"",
											'Category', m.""Category"",						
											'EntityId', m.""EntityId"",
											'EntityName', m.""EntityName"",
											'CreatedAt', m.""CreatedAt"",
											'CreatedBy', m.""CreatedBy"",
											'UpdatedAt', m.""UpdatedAt"",
											'UpdatedBy', m.""UpdatedBy"",
											'Species', jsonb_build_object(
												'Id', s.""Id"",
												'CommonName', s.""CommonName"",
												'ScientificName', s.""ScientificName"",
												'Type', s.""Type"",
												'CreatedAt', s.""CreatedAt"",
												'CreatedBy', s.""CreatedBy""
											)
										)
									), '[]'::JSONB)
									FROM ""MigratoryBirds"" m
									JOIN ""Species"" s ON s.""Id"" = m.""SpeciesId""
									WHERE m.""EntityId"" = lt.""Id""
									AND m.""Category"" = 'LineTransectStart'
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
									WHERE u.""Id"" = lt.""UpdatedBy""
								)
							),
							'{}'::JSONB
						) INTO response_data
						FROM ""LineTransects"" lt
						JOIN ""Parks"" p ON p.""Id"" = lt.""ParkId""
						JOIN ""Locations"" ltl ON ltl.""Id"" = lt.""LineTransectLocalAreaNameId""
						JOIN ""Locations"" alt ON alt.""Id"" = lt.""AlongTransectLocalAreaNameId""
						JOIN ""Users"" u ON u.""Id"" = lt.""CreatedBy""
						WHERE lt.""Id"" = line_transect_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::JSONB);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_line_transect_by_id: %', SQLERRM;
					END;				
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_bird_surveys(
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
						
						-- bird_survey_data
						WITH bird_survey_data AS (
							SELECT 
								bs.""Id"",
								bs.""LocalAreaNameId"",
								bs.""Method"",
								bs.""Habitat"",
								bs.""CreatedAt"",
								bs.""CreatedBy"",
								(
									SELECT COALESCE(SUM(mb.""IndividualObserved""), 0)
									FROM ""MigratoryBirds"" mb
									WHERE mb.""EntityId"" = bs.""Id""
								) AS ""TotalIndividual"",
								loc.""Id"" AS location_id,
								loc.""Name"" AS location_name,
								p.""Id"" AS parkId,
								p.""Name"" AS parkName,
								u.""Id"" AS user_id,
								u.""Username"" AS username,
								ROW_NUMBER() OVER (ORDER BY bs.""CreatedAt"" DESC) AS row_number,
								-- Get total count
								COUNT(*) OVER() AS full_count
							FROM ""BirdSurveys"" bs
							JOIN ""Locations"" loc ON loc.""Id"" = bs.""LocalAreaNameId""
							JOIN ""Parks"" p ON p.""Id"" = bs.""ParkId""
							JOIN ""Users"" u ON u.""Id"" = bs.""CreatedBy""
							WHERE
								(search_text IS NULL OR search_text = '' OR
								loc.""Name"" ILIKE '%' || search_text || '%')
							AND bs.""DeletedAt"" IS NULL
							AND (park_id IS NULL OR park_id = '' OR bs.""ParkId"" = park_id::UUID)
							AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR
								bs.""ParkId"" = ANY(park_ids))
						),
						paginated_data AS (
							SELECT * FROM bird_survey_data
							WHERE row_number BETWEEN min_row_num AND max_row_num
						)
						SELECT 
							jsonb_agg(
								jsonb_build_object(
									'RowNumber', row_number,
									'Id', ""Id"",
									'LocalAreaNameId', ""LocalAreaNameId"",
									'Method', ""Method"",
									'Habitat', ""Habitat"",
									'TotalIndividual', ""TotalIndividual"",
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
							RAISE EXCEPTION 'Error in fn_bird_surveys: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_bird_survey(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_bird_survey_id UUID;
						v_migratory_bird_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'BirdSurvey';
						v_migratory_bird_entity CONSTANT VARCHAR(50) := 'MigratoryBird';
						v_species_observed_element JSONB;
						v_park_id UUID;
					BEGIN  
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Insert BirdSurveys data
						BEGIN
							INSERT INTO public.""BirdSurveys""(
								""Id"",
								""Session"",
								""LocalAreaNameId"",
								""ParkId"",
								""Method"", 
								""Habitat"",
								""Coordinates"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								input_data->>'Session',
								(input_data->>'LocalAreaNameId')::UUID,
								v_park_id,
								input_data->>'Method',
								input_data->>'Habitat',				
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_bird_survey_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'BirdSurveys table error: %', SQLERRM;
						END;

						-- Insert SpeciesObserved Data
						IF input_data->'SpeciesObserved' IS NOT NULL AND jsonb_array_length(input_data->'SpeciesObserved') > 0 THEN
						    FOR v_species_observed_element IN
						        SELECT * FROM jsonb_array_elements(input_data->'SpeciesObserved')
						    LOOP
						        BEGIN
									INSERT INTO public.""MigratoryBirds""(
										""Id"", 
										""SpeciesId"", 
										""MigrationType"", 
										""Arrival"", 
										""Activity"", 
										""IndividualObserved"",
										""Remark"", 
										""Category"", 
										""EntityId"", 
										""EntityName"",
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_species_observed_element->>'SpeciesId')::UUID,
										v_species_observed_element->>'MigrationType',
										v_species_observed_element->>'Arrival',
										v_species_observed_element->>'Activity',
										(v_species_observed_element->>'IndividualObserved')::INT,
										v_species_observed_element->>'Remark',
										v_entity_name,
										v_bird_survey_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									) RETURNING ""Id"" INTO v_migratory_bird_id;
									
									-- Insert Files Data
									DECLARE v_image_id TEXT;
						            BEGIN
						                v_image_id := v_species_observed_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
						                        ""EntityId"" = v_migratory_bird_id,
						                        ""EntityName"" = v_migratory_bird_entity,
						                        ""UpdatedBy"" = (input_data->>'CreatedBy')::UUID,
						                        ""UpdatedAt"" = NOW()
						                    WHERE ""Id"" = v_image_id::UUID;
						                END IF;
										EXCEPTION
								            WHEN OTHERS THEN
								                RAISE EXCEPTION 'Files (SpeciesObserved) table error: %', SQLERRM;
						            END;
									
						        EXCEPTION
						            WHEN OTHERS THEN
						                RAISE EXCEPTION 'MigratoryBirds (SpeciesObserved) table error: %', SQLERRM;
						        END;
						    END LOOP;
						END IF;
						
						-- Return the created v_bird_survey_id
						RETURN v_bird_survey_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_bird_survey: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_bird_survey_by_id(
					bird_survey_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', bs.""Id"",
								'Session', bs.""Session"",
								'ParkId', bs.""ParkId"",
								'LocalAreaNameId', bs.""LocalAreaNameId"",
								'Session', bs.""Session"",
								'Method', bs.""Method"",
								'Habitat', bs.""Habitat"",
								'Coordinates', bs.""Coordinates"",	
								'CreatedBy', bs.""CreatedBy"",
								'CreatedAt', bs.""CreatedAt"",
								'UpdatedBy', bs.""UpdatedBy"",
								'UpdatedAt', bs.""UpdatedAt"",
								'TotalIndividual', (
									SELECT COALESCE(SUM(mb.""IndividualObserved""), 0)
									FROM ""MigratoryBirds"" mb
									WHERE mb.""EntityId"" = bs.""Id""
								),
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'Location', jsonb_build_object(
									'Id', l.""Id"",
									'Name', l.""Name"",
									'ParkId', l.""ParkId"",
									'CreatedBy', l.""CreatedBy"",
									'CreatedAt', l.""CreatedAt""
								),
								'MigratoryBirds', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', m.""Id"",
											'SpeciesId', m.""SpeciesId"",
											'MigrationType', m.""MigrationType"",
											'Arrival', m.""Arrival"",
											'Activity', m.""Activity"",
											'IndividualObserved', m.""IndividualObserved"",
											'Remark', m.""Remark"",
											'Category', m.""Category"",						
											'EntityId', m.""EntityId"",
											'EntityName', m.""EntityName"",
											'CreatedAt', m.""CreatedAt"",
											'CreatedBy', m.""CreatedBy"",
											'UpdatedAt', m.""UpdatedAt"",
											'UpdatedBy', m.""UpdatedBy"",
											'Species', jsonb_build_object(
												'Id', s.""Id"",
												'CommonName', s.""CommonName"",
												'ScientificName', s.""ScientificName"",
												'Type', s.""Type"",
												'CreatedAt', s.""CreatedAt"",
												'CreatedBy', s.""CreatedBy""
											)
										)
									), '[]'::JSONB)
									FROM ""MigratoryBirds"" m
									JOIN ""Species"" s ON s.""Id"" = m.""SpeciesId""
									WHERE m.""EntityId"" = bs.""Id""
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
									WHERE u.""Id"" = bs.""UpdatedBy""
								)
							),
							'{}'::JSONB
						) INTO response_data
						FROM ""BirdSurveys"" bs
						JOIN ""Parks"" p ON p.""Id"" = bs.""ParkId""
						JOIN ""Locations"" l ON l.""Id"" = bs.""LocalAreaNameId""
						JOIN ""Users"" u ON u.""Id"" = bs.""CreatedBy""
						WHERE bs.""Id"" = bird_survey_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::JSONB);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_bird_survey_by_id: %', SQLERRM;
					END;				
				$BODY$;
            ");

            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_bird_survey(
					input_data jsonb,
					bird_survey_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_bird_survey_id UUID;
						v_migratory_bird_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'BirdSurvey';
						v_migratory_bird_entity CONSTANT VARCHAR(50) := 'MigratoryBird';
						v_species_observed_element JSONB;
						v_park_id UUID;
					BEGIN 
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalAreaNameId')::UUID;
						
						-- Update BirdSurveys data
						BEGIN
							-- Convert v_bird_survey_id parameter to UUID
							v_bird_survey_id := bird_survey_id::UUID;
							
							UPDATE public.""BirdSurveys""
							SET
								""LocalAreaNameId"" = (input_data->>'LocalAreaNameId')::UUID,
								""ParkId"" = v_park_id,
								""Session"" = input_data->>'Session',
								""Method"" = input_data->>'Method',
								""Habitat"" = input_data->>'Habitat',
								""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_bird_survey_id
							RETURNING ""Id"" INTO v_bird_survey_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'BirdSurveys table error: %', SQLERRM;
						END;

						-- Update SpeciesObserved Data
						BEGIN
							IF input_data->'SpeciesObserved' IS NOT NULL AND jsonb_array_length(input_data->'SpeciesObserved') > 0 THEN
								
								-- Delete existing MigratoryBirds
								DELETE FROM public.""MigratoryBirds""
								WHERE ""EntityId"" = v_bird_survey_id;
							
								-- Insert MigratoryBirds data
								FOR v_species_observed_element IN
									SELECT * FROM jsonb_array_elements(input_data->'SpeciesObserved')
								LOOP
									INSERT INTO public.""MigratoryBirds""(
										""Id"", 
										""SpeciesId"", 
										""MigrationType"", 
										""Arrival"",
										""Activity"",
										""IndividualObserved"", 
										""Remark"", 
										""Category"", 
										""EntityId"", 
										""EntityName"",
										""CreatedBy"",
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_species_observed_element->>'SpeciesId')::UUID,
										v_species_observed_element->>'MigrationType',
										v_species_observed_element->>'Arrival',
										v_species_observed_element->>'Activity',
										(v_species_observed_element->>'IndividualObserved')::INT,
										v_species_observed_element->>'Remark',
										v_entity_name,
										v_bird_survey_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									)RETURNING ""Id"" INTO v_migratory_bird_id;
									
									-- Insert Files Data
									DECLARE v_image_id TEXT;
						            BEGIN
						                v_image_id := v_species_observed_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
						                        ""EntityId"" = v_migratory_bird_id,
						                        ""EntityName"" = v_migratory_bird_entity,
						                        ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
						                        ""UpdatedAt"" = NOW()
						                    WHERE ""Id"" = v_image_id::UUID;
						                END IF;
										EXCEPTION
								            WHEN OTHERS THEN
								                RAISE EXCEPTION 'Files (SpeciesObserved) table error: %', SQLERRM;
						            END;
									
								END LOOP;
							END IF;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'MigratoryBirds (SpeciesObserved) table error: %', SQLERRM;
						END;

						-- Return the updated v_bird_survey_id
						RETURN v_bird_survey_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_bird_survey: %', SQLERRM;
					END;		
				$BODY$;
			");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_line_transects(integer, integer, character varying, character varying, uuid[]);
				DROP FUNCTION IF EXISTS public.fn_create_line_transect(jsonb);
				DROP FUNCTION IF EXISTS public.fn_line_transect_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_line_transect(jsonb, character varying);
				DROP FUNCTION IF EXISTS public.fn_bird_surveys(integer, integer, character varying, character varying, uuid[]);
				DROP FUNCTION IF EXISTS public.fn_create_bird_survey(jsonb);
				DROP FUNCTION IF EXISTS public.fn_bird_survey_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_bird_survey(jsonb, character varying);
            ");
        }
    }
}
