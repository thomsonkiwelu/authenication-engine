using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateRareEndangeredSpeciesFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_rare_endangered_species(
					page_number integer,
					page_size integer,
					search_text character varying DEFAULT ''::character varying,
					park_id character varying DEFAULT ''::character varying,
					category character varying DEFAULT ''::character varying,
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
						WITH rare_endangered_species_data AS (
							SELECT 
								res.""Id"",
								res.""ParkId"",
								res.""Session"",
								res.""Category"",
								res.""CreatedAt"",
								res.""CreatedBy"",
								(
									SELECT COALESCE(SUM(rs.""NumberOfIndividual""), 0)
									FROM ""RareSpeciesOccurrences"" rs
									WHERE rs.""RareEndangeredSpeciesId"" = res.""Id""
								) AS TotalIndividual,
								(
								    SELECT COALESCE(
								        string_agg(ec.""Value"", ', ' ORDER BY ec.""Value""), 
								        ''
								    )
								    FROM ""EcologySelections"" ec
								    WHERE ec.""EntityId"" = res.""Id""
								) AS SourceOfInformation,
								p.""Id"" AS parkId,
								p.""Name"" AS parkName,
								u.""Id"" AS user_id,
								u.""Username"" AS username,
								ROW_NUMBER() OVER (ORDER BY res.""CreatedAt"" DESC) AS row_number,
								-- Get total count
								COUNT(*) OVER() AS full_count
							FROM ""RareEndangeredSpecies"" res
							JOIN ""Parks"" p ON p.""Id"" = res.""ParkId""
							JOIN ""Users"" u ON u.""Id"" = res.""CreatedBy""
							WHERE
								(search_text IS NULL OR search_text = '' OR
								p.""Name"" ILIKE '%' || search_text || '%')
							AND res.""DeletedAt"" IS NULL
							AND (category IS NULL OR category = '' OR res.""Category"" = category)
							AND (park_id IS NULL OR park_id = '' OR res.""ParkId"" = park_id::UUID)
							AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR
								res.""ParkId"" = ANY(park_ids))
						),
						paginated_data AS (
							SELECT * FROM rare_endangered_species_data
							WHERE row_number BETWEEN min_row_num AND max_row_num
						)
						SELECT 
							jsonb_agg(
								jsonb_build_object(
									'RowNumber', row_number,
									'Id', ""Id"",
									'ParkId', ""ParkId"",
									'Session', ""Session"",
									'Category', ""Category"",
									'KeySourceOfInformation', SourceOfInformation,
									'TotalIndividual', TotalIndividual,
									'CreatedAt', ""CreatedAt"",
									'CreatedBy', ""CreatedBy"",
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
							RAISE EXCEPTION 'Error in fn_rare_endangered_species: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_rare_endangered_species_by_id(
					rare_endangered_species_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', res.""Id"",
								'ParkId', res.""ParkId"",
								'Session', res.""Session"",
								'Category', res.""Category"",
								'NameKeyInformer', res.""NameKeyInformer"",
								'CreatedBy', res.""CreatedBy"",
								'CreatedAt', res.""CreatedAt"",
								'UpdatedBy', res.""UpdatedBy"",
								'UpdatedAt', res.""UpdatedAt"",
								'TotalIndividual', (
									SELECT COALESCE(SUM(rs.""NumberOfIndividual""), 0)
									FROM ""RareSpeciesOccurrences"" rs
									WHERE rs.""RareEndangeredSpeciesId"" = res.""Id""
								),
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'RareSpeciesOccurrences', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', rso.""Id"",
											'SpeciesId', rso.""SpeciesId"",
											'LocalAreaNameId', rso.""LocalAreaNameId"",
											'RareEndangeredSpeciesId', rso.""RareEndangeredSpeciesId"",
											'VegetationCategory', rso.""VegetationCategory"",
											'NumberOfIndividual', rso.""NumberOfIndividual"",
											'Coordinates', rso.""Coordinates"",
											'Remark', rso.""Remark"",				
											'CreatedAt', rso.""CreatedAt"",
											'CreatedBy', rso.""CreatedBy"",
											'UpdatedAt', rso.""UpdatedAt"",
											'UpdatedBy', rso.""UpdatedBy"",
											'Species', jsonb_build_object(
												'Id', s.""Id"",
												'CommonName', s.""CommonName"",
												'ScientificName', s.""ScientificName"",
												'Type', s.""Type"",
												'CreatedAt', s.""CreatedAt"",
												'CreatedBy', s.""CreatedBy""
											),
											'Location', jsonb_build_object(
												'Id', l.""Id"",
												'Name', l.""Name"",
												'ParkId', l.""ParkId"",
												'CreatedBy', l.""CreatedBy"",
												'CreatedAt', l.""CreatedAt""
											)
										)
									), '[]'::JSONB)
									FROM ""RareSpeciesOccurrences"" rso
									JOIN ""Species"" s ON s.""Id"" = rso.""SpeciesId""
									JOIN ""Locations"" l ON l.""Id"" = rso.""LocalAreaNameId""
									WHERE rso.""RareEndangeredSpeciesId"" = res.""Id""
								),
								'SourceOfInformation', (
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
									WHERE ec.""EntityId"" = res.""Id""
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
									WHERE u.""Id"" = res.""UpdatedBy""
								)
							),
							'{}'::JSONB
						) INTO response_data
						FROM ""RareEndangeredSpecies"" res
						JOIN ""Parks"" p ON p.""Id"" = res.""ParkId""
						JOIN ""Users"" u ON u.""Id"" = res.""CreatedBy""
						WHERE res.""Id"" = rare_endangered_species_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::JSONB);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_rare_endangered_species_by_id: %', SQLERRM;
					END;				
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_rare_endangered_species(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_rare_endangered_species_id UUID;
						v_rare_species_occurrences_id UUID;
						v_rare_endangered_species_entity CONSTANT VARCHAR(50) := 'RareEndangeredSpecies';
						v_rare_species_occurrence_entity CONSTANT VARCHAR(50) := 'RareSpeciesOccurrence';
						v_animal_observed_element JSONB;
						v_array_element TEXT;
						v_image_id TEXT;
					BEGIN  
						-- Insert RareEndangeredSpecies data
						BEGIN
							INSERT INTO public.""RareEndangeredSpecies""(
								""Id"",
								""ParkId"", 
								""Session"",
								""Category"", 
								""NameKeyInformer"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'ParkId')::UUID,
								input_data->>'Session',
								input_data->>'Category',
								NULLIF(COALESCE(input_data->>'NameKeyInformer', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_rare_endangered_species_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'RareEndangeredSpecies table error: %', SQLERRM;
						END;

						-- Insert AnimalObserved Data
						IF input_data->'AnimalObserved' IS NOT NULL AND jsonb_array_length(input_data->'AnimalObserved') > 0 THEN
						    FOR v_animal_observed_element IN
						        SELECT * FROM jsonb_array_elements(input_data->'AnimalObserved')
						    LOOP
						        BEGIN
									INSERT INTO public.""RareSpeciesOccurrences""(
										""Id"",
										""LocalAreaNameId"",
										""SpeciesId"",
										""RareEndangeredSpeciesId"",
										""VegetationCategory"",
										""NumberOfIndividual"", 
										""Remark"",
										""Coordinates"",
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_animal_observed_element->>'LocalAreaNameId')::UUID,
										(v_animal_observed_element->>'SpeciesId')::UUID,
										v_rare_endangered_species_id,
										v_animal_observed_element->>'VegetationCategory',
										(v_animal_observed_element->>'NumberOfIndividual')::INT,
										v_animal_observed_element->>'Remark',
										NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
										(input_data->>'CreatedBy')::UUID,
										NOW()
									) RETURNING ""Id"" INTO v_rare_species_occurrences_id;
									
									-- Insert Files Data
						            BEGIN
						                v_image_id := v_animal_observed_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
						                        ""EntityId"" = v_rare_species_occurrences_id,
						                        ""EntityName"" = v_rare_species_occurrence_entity,
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
						                RAISE EXCEPTION 'RareSpeciesOccurrences table error: %', SQLERRM;
						        END;
						    END LOOP;
						END IF;

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
										v_rare_endangered_species_id,
										v_rare_endangered_species_entity,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'EcologySelections table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return the created v_rare_endangered_species_id
						RETURN v_rare_endangered_species_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_rare_endangered_species: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_rare_endangered_species(
					input_data jsonb,
					rare_endangered_species_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_rare_endangered_species_id UUID;
						v_rare_species_occurrences_id UUID;
						v_rare_endangered_species_entity CONSTANT VARCHAR(50) := 'RareEndangeredSpecies';
						v_rare_species_occurrence_entity CONSTANT VARCHAR(50) := 'RareSpeciesOccurrence';
						v_animal_observed_element JSONB;
						v_array_element TEXT;
						v_image_id TEXT;
					BEGIN 
						-- Update RareEndangeredSpecies data
						BEGIN
							-- Convert v_rare_endangered_species_id parameter to UUID
							v_rare_endangered_species_id := rare_endangered_species_id::UUID;
							
							UPDATE public.""RareEndangeredSpecies""
							SET
								""ParkId"" = (input_data->>'ParkId')::UUID,
								""Session"" = input_data->>'Session',
								""Category"" = input_data->>'Category',
								""NameKeyInformer"" = NULLIF(COALESCE(input_data->>'NameKeyInformer', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_rare_endangered_species_id
							RETURNING ""Id"" INTO v_rare_endangered_species_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'RareEndangeredSpecies table error: %', SQLERRM;
						END;

						-- Update AnimalObserved Data
						BEGIN
							IF input_data->'AnimalObserved' IS NOT NULL AND jsonb_array_length(input_data->'AnimalObserved') > 0 THEN
								
								-- Delete existing RareSpeciesOccurrences
								DELETE FROM public.""RareSpeciesOccurrences""
								WHERE ""RareEndangeredSpeciesId"" = v_rare_endangered_species_id;
							
								-- Insert RareSpeciesOccurrences data
								FOR v_animal_observed_element IN
									SELECT * FROM jsonb_array_elements(input_data->'AnimalObserved')
								LOOP
									INSERT INTO public.""RareSpeciesOccurrences""(
										""Id"",
										""LocalAreaNameId"",
										""SpeciesId"",
										""RareEndangeredSpeciesId"",
										""VegetationCategory"",
										""NumberOfIndividual"", 
										""Remark"",
										""Coordinates"",
										""CreatedBy"",
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_animal_observed_element->>'LocalAreaNameId')::UUID,
										(v_animal_observed_element->>'SpeciesId')::UUID,
										v_rare_endangered_species_id,
										v_animal_observed_element->>'VegetationCategory',
										(v_animal_observed_element->>'NumberOfIndividual')::INT,
										v_animal_observed_element->>'Remark',
										NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									) RETURNING ""Id"" INTO v_rare_species_occurrences_id;
									
									-- Insert Files Data
						            BEGIN
						                v_image_id := v_animal_observed_element->>'ImageId';
						                IF v_image_id IS NOT NULL AND v_image_id != '' THEN
						                    UPDATE public.""Files""
						                    SET
						                        ""EntityId"" = v_rare_species_occurrences_id,
						                        ""EntityName"" = v_rare_species_occurrence_entity,
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
								RAISE EXCEPTION 'RareSpeciesOccurrences table error: %', SQLERRM;
						END;

						-- Update SourceOfInformation Data
						BEGIN
							IF input_data->'SourceOfInformation' IS NOT NULL AND jsonb_array_length(input_data->'SourceOfInformation') > 0 THEN
							
								-- Delete existing SourceOfInformation
								DELETE FROM public.""EcologySelections""
								WHERE ""EntityId"" = v_rare_endangered_species_id 
								AND ""EntityName"" = v_rare_endangered_species_entity;
								
								-- Insert SourceOfInformation
								FOR v_array_element IN 
									SELECT jsonb_array_elements_text(input_data->'SourceOfInformation')
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
										'SourceOfInformation',
										v_rare_endangered_species_id,
										v_rare_endangered_species_entity,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								END LOOP;
							END IF;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'EcologySelections table error: %', SQLERRM;
						END;

						-- Return the updated v_rare_endangered_species_id
						RETURN v_rare_endangered_species_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_rare_endangered_species: %', SQLERRM;
					END;		
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_rare_endangered_species(integer, integer, character varying, character varying, character varying, uuid[]);
				DROP FUNCTION IF EXISTS public.fn_rare_endangered_species_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_create_rare_endangered_species(jsonb);
				DROP FUNCTION IF EXISTS public.fn_update_rare_endangered_species(jsonb, character varying);
            ");
        }
    }
}
