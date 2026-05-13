using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateAerialCensusFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_aerial_censuses(
					page_number integer,
					page_size integer,
					start_date character varying DEFAULT ''::character varying,
					end_date character varying DEFAULT ''::character varying,
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
						
						-- aerial_censuses_data
						WITH aerial_censuses_data AS (
							SELECT 
								ac.""Id"",
								ac.""ParkId"",
								ac.""AreaInvolved"",
								ac.""CensusType"",
								ac.""StartDate"",
								ac.""EndDate"",
								ac.""CreatedAt"",
								ac.""CreatedBy"",
								(
			    					SELECT COALESCE(SUM(ad.""ObservedNumber""), 0)
							        FROM ""SpeciesOccurrences"" ad
							        WHERE ad.""EntityId"" = ac.""Id""
							    ) AS ""TotalObservedSpecies"",
								(
			    					SELECT COALESCE(SUM(ad.""EstimatedNumber""), 0)
							        FROM ""SpeciesOccurrences"" ad
							        WHERE ad.""EntityId"" = ac.""Id""
							    ) AS ""TotalEstimatedSpecies"",
								p.""Id"" AS parkId,
								p.""Name"" AS parkName,
								u.""Id"" AS user_id,
								u.""Username"" AS username,
								ROW_NUMBER() OVER (ORDER BY ac.""CreatedAt"" DESC) AS row_number,
								-- Get total count
								COUNT(*) OVER() AS full_count
							FROM ""AerialCensuses"" ac
							JOIN ""Parks"" p ON p.""Id"" = ac.""ParkId""
							JOIN ""Users"" u ON u.""Id"" = ac.""CreatedBy""
							AND ac.""DeletedAt"" IS NULL
							AND ((start_date IS NULL OR start_date = '') OR ac.""StartDate"" >= (start_date::DATE))
							AND ((end_date IS NULL OR end_date = '') OR ac.""EndDate"" <= (end_date::DATE))
							AND (park_id IS NULL OR park_id = '' OR ac.""ParkId"" = park_id::UUID)
							AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR
     							ac.""ParkId"" = ANY(park_ids))
						),
						paginated_data AS (
							SELECT * FROM aerial_censuses_data
							WHERE row_number BETWEEN min_row_num AND max_row_num
						)
						SELECT 
							jsonb_agg(
								jsonb_build_object(
									'RowNumber', row_number,
									'Id', ""Id"",
									'ParkId', ""ParkId"",
									'AreaInvolved', ""AreaInvolved"",
									'CensusType', ""CensusType"",
									'StartDate', ""StartDate"",
									'EndDate', ""EndDate"",
									'TotalObservedSpecies', ""TotalObservedSpecies"",
									'TotalEstimatedSpecies', ""TotalEstimatedSpecies"",
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
							RAISE EXCEPTION 'Error in fn_aerial_censuses: %', SQLERRM;
					END;        
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_create_aerial_census(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
				    DECLARE
				        v_aerial_census_id UUID;
				        v_entity_name CONSTANT VARCHAR(50) := 'AerialCensuses';
				        v_species_observed_element JSONB;
				    BEGIN  

				        -- Insert AerialCensuses data
				        BEGIN
				            INSERT INTO public.""AerialCensuses""(
								""Id"",
								""ParkId"",
								""AreaInvolved"",
								""AreaCovered"",
								""CensusType"",
								""Session"",
								""StartDate"",
								""EndDate"",
								""Coordinates"",
								""CreatedBy"",
								""CreatedAt""
				            ) VALUES (
				                gen_random_uuid(),
				                (input_data->>'ParkId')::UUID,
								input_data->>'AreaInvolved',
								(input_data->>'AreaCovered')::NUMERIC,
								input_data->>'CensusType',
								input_data->>'Session',
								(input_data->>'StartDate')::DATE,
								(input_data->>'EndDate')::DATE,
				                NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
				                (input_data->>'CreatedBy')::UUID,
				                NOW()
				            ) RETURNING ""Id"" INTO v_aerial_census_id;
				        EXCEPTION
				            WHEN OTHERS THEN
				                RAISE EXCEPTION 'AerialCensuses table error: %', SQLERRM;
				        END;

				        -- Insert SpeciesOccurrences Data
				        IF input_data->'SpeciesObserved' IS NOT NULL AND jsonb_array_length(input_data->'SpeciesObserved') > 0 THEN
				            FOR v_species_observed_element IN
				                SELECT * FROM jsonb_array_elements(input_data->'SpeciesObserved')
				            LOOP
				                BEGIN
				                    INSERT INTO public.""SpeciesOccurrences""(
				                        ""Id"",
				                        ""SpeciesId"",
				                        ""EntityId"",
				                        ""EntityName"", 
										""ObservedNumber"", 
										""EstimatedNumber"",
										""StandardError"",
				                        ""CreatedBy"",
				                        ""CreatedAt""
				                    ) VALUES (
				                        gen_random_uuid(),
				                        (v_species_observed_element->>'SpeciesId')::UUID,
				                        v_aerial_census_id,
				                        v_entity_name,
										(v_species_observed_element->>'ObservedNumber')::NUMERIC,
										(v_species_observed_element->>'EstimatedNumber')::NUMERIC,
										v_species_observed_element->>'StandardError',
				                        (input_data->>'CreatedBy')::UUID,
				                        NOW()
				                    );
				                EXCEPTION
				                    WHEN OTHERS THEN
				                        RAISE EXCEPTION 'SpeciesOccurrences table error: %', SQLERRM;
				                END;
				            END LOOP;
				        END IF;
				        
				        -- Return the created v_aerial_census_id
				        RETURN v_aerial_census_id::TEXT;
				        
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'Error in fn_create_aerial_census: %', SQLERRM;
				    END;        
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_update_aerial_census(
					input_data jsonb,
					aerial_census_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE
				    v_aerial_census_id UUID;
				    v_entity_name CONSTANT VARCHAR(50) := 'AerialCensuses';
				    v_species_observed_element JSONB;
				BEGIN 
				    -- Update AerialCensuses data
				    BEGIN
				        -- Convert v_aerial_census_id parameter to UUID
				        v_aerial_census_id := aerial_census_id::UUID;
						
				        UPDATE public.""AerialCensuses""
				        SET
							""ParkId"" = (input_data->>'ParkId')::UUID,
				            ""AreaInvolved"" = input_data->>'AreaInvolved',
							""AreaCovered"" = (input_data->>'AreaCovered')::NUMERIC,
							""CensusType"" = input_data->>'CensusType',
							""Session"" = input_data->>'Session',
							""StartDate"" = (input_data->>'StartDate')::DATE,
							""EndDate"" = (input_data->>'EndDate')::DATE,
				            ""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
				            ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
				            ""UpdatedAt"" = NOW()
				        WHERE ""Id"" = v_aerial_census_id
				        RETURNING ""Id"" INTO v_aerial_census_id;
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'AerialCensuses table error: %', SQLERRM;
				    END;

				    -- Update SpeciesOccurrences Data
				    BEGIN
				        IF input_data->'SpeciesObserved' IS NOT NULL AND jsonb_array_length(input_data->'SpeciesObserved') > 0 THEN
				            
				            -- Delete existing SpeciesOccurrences
				            DELETE FROM public.""SpeciesOccurrences""
				            WHERE ""EntityId"" = v_aerial_census_id;
				                
				            -- Insert v_aerial_census_id data	
				            FOR v_species_observed_element IN
				                SELECT * FROM jsonb_array_elements(input_data->'SpeciesObserved')
				            LOOP
								INSERT INTO public.""SpeciesOccurrences""(
									""Id"",
									""SpeciesId"",
									""EntityId"",
									""EntityName"", 
									""ObservedNumber"", 
									""EstimatedNumber"",
									""StandardError"",
									""CreatedBy"",
									""CreatedAt"",
									""UpdatedBy"",
				                    ""UpdatedAt""
								) VALUES (
									gen_random_uuid(),
									(v_species_observed_element->>'SpeciesId')::UUID,
									v_aerial_census_id,
									v_entity_name,
									(v_species_observed_element->>'ObservedNumber')::NUMERIC,
									(v_species_observed_element->>'EstimatedNumber')::NUMERIC,
									v_species_observed_element->>'StandardError',
									(input_data->>'UpdatedBy')::UUID,
									NOW(),
									(input_data->>'UpdatedBy')::UUID,
				                    NOW()
								);
				            END LOOP;
				        END IF;
					    EXCEPTION
					        WHEN OTHERS THEN
					            RAISE EXCEPTION 'SpeciesOccurrences table error: %', SQLERRM;
				    END;

				    -- Return the updated v_aerial_census_id
				    RETURN v_aerial_census_id::TEXT;
				    
				EXCEPTION
				    WHEN OTHERS THEN
				        RAISE EXCEPTION 'Error in fn_update_aerial_census: %', SQLERRM;
				END;        
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_aerial_census_by_id(
					aerial_census_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', ac.""Id"",
								'ParkId', ac.""ParkId"",
								'AreaInvolved', ac.""AreaInvolved"",
								'AreaCovered', ac.""AreaCovered"",
								'CensusType', ac.""CensusType"",
								'Session', ac.""Session"",
								'StartDate', ac.""StartDate"",
								'EndDate', ac.""EndDate"",
								'Coordinates', ac.""Coordinates"",
								'CreatedBy', ac.""CreatedBy"",
								'CreatedAt', ac.""CreatedAt"",
								'UpdatedAt', ac.""UpdatedAt"",
								'UpdatedBy', ac.""UpdatedBy"",
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'SpeciesOccurrences', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', spo.""Id"",
											'SpeciesId', spo.""SpeciesId"",
											'EntityId', spo.""EntityId"",
											'EntityName', spo.""EntityName"",
											'ObservedNumber', spo.""ObservedNumber"",
											'EstimatedNumber', spo.""EstimatedNumber"",
											'StandardError', spo.""StandardError"",
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
									WHERE spo.""EntityId"" = ac.""Id""
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
									WHERE u.""Id"" = ac.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""AerialCensuses"" ac
						JOIN ""Parks"" p ON p.""Id"" = ac.""ParkId""
						JOIN ""Users"" u ON u.""Id"" = ac.""CreatedBy""
						WHERE ac.""Id"" = aerial_census_id;
						
						-- data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_aerial_census_by_id: %', SQLERRM;
					END;		
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_aerial_censuses(integer, integer, character varying, character varying, character varying, uuid[]);
				DROP FUNCTION IF EXISTS public.fn_create_aerial_census(jsonb);
				DROP FUNCTION IF EXISTS public.fn_aerial_census_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_aerial_census(jsonb, character varying);
            ");
        }
    }
}
