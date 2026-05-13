using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateWasteManagementFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_wastes(
					page_number integer,
					page_size integer,
					search_text character varying DEFAULT ''::character varying,
					category character varying DEFAULT ''::character varying,
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
						
						-- waste_data
						WITH waste_data AS (
							SELECT 
								wa.""Id"",
								wa.""StationId"",
								wa.""Category"",
								wa.""CreatedAt"",
								wa.""CreatedBy"",
								(
			    					SELECT COALESCE(SUM(wm.""Quantity""), 0)
							        FROM ""WasteMaterials"" wm
							        WHERE wm.""WasteId"" = wa.""Id""
									AND wm.""State"" = 'Solid'
							    ) AS ""TotalSolidState"",
								(
			    					SELECT COALESCE(SUM(wm.""Quantity""), 0)
							        FROM ""WasteMaterials"" wm
							        WHERE wm.""WasteId"" = wa.""Id""
									AND wm.""State"" = 'Liquid'
							    ) AS ""TotalLiquidState"",
								st.""Id"" AS station_id,
								st.""Name"" AS station_name,
								st.""Type"" AS station_type,
								p.""Id"" AS parkId,
								p.""Name"" AS parkName,
								u.""Id"" AS user_id,
								u.""Username"" AS username,
								ROW_NUMBER() OVER (ORDER BY wa.""CreatedAt"" DESC) AS row_number,
								-- Get total count
								COUNT(*) OVER() AS full_count
							FROM ""Wastes"" wa
							JOIN ""Stations"" st ON st.""Id"" = wa.""StationId""
							JOIN ""Parks"" p ON p.""Id"" = wa.""ParkId""
							JOIN ""Users"" u ON u.""Id"" = wa.""CreatedBy""
							WHERE
								(search_text IS NULL OR search_text = '' OR
								st.""Name"" ILIKE '%' || search_text || '%')
							AND wa.""DeletedAt"" IS NULL
							AND (category IS NULL OR category = '' OR wa.""Category"" = category)
							AND (park_id IS NULL OR park_id = '' OR wa.""ParkId"" = park_id::UUID)
							AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR
     						wa.""ParkId"" = ANY(park_ids))
						),
						paginated_data AS (
							SELECT * FROM waste_data
							WHERE row_number BETWEEN min_row_num AND max_row_num
						)
						SELECT 
							jsonb_agg(
								jsonb_build_object(
									'RowNumber', row_number,
									'Id', ""Id"",
									'Category', ""Category"",
									'StationId', ""StationId"",
									'TotalSolidState', ""TotalSolidState"",
									'TotalLiquidState', ""TotalLiquidState"",
									'CreatedAt', ""CreatedAt"",
									'CreatedBy', ""CreatedBy"",
									'Station', jsonb_build_object(
										'Id', station_id,
										'Name', station_name,
										'Type', station_type
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
							RAISE EXCEPTION 'Error in fn_wastes: %', SQLERRM;
					END;        
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_waste_by_id(
					waste_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', w.""Id"",
								'StationId', w.""StationId"",
								'Category', w.""Category"",
								'SolidStateRemark', w.""SolidStateRemark"",
								'LiquidStateRemark', w.""LiquidStateRemark"",
								'Coordinates', w.""Coordinates"",
								'CreatedBy', w.""CreatedBy"",
								'CreatedAt', w.""CreatedAt"",
								'UpdatedBy', w.""UpdatedBy"",
								'UpdatedAt', w.""UpdatedAt"",
								'TotalSolidState',(
			    					SELECT COALESCE(SUM(wm.""Quantity""), 0)
							        FROM ""WasteMaterials"" wm
							        WHERE wm.""WasteId"" = w.""Id""
									AND wm.""State"" = 'Solid'
							    ),
								'TotalLiquidState',(
			    					SELECT COALESCE(SUM(wm.""Quantity""), 0)
							        FROM ""WasteMaterials"" wm
							        WHERE wm.""WasteId"" = w.""Id""
									AND wm.""State"" = 'Liquid'
							    ),
								'Station', jsonb_build_object(
									'Id', st.""Id"",
									'Name', st.""Name"",
									'Type', st.""Type"",
									'ParkId', st.""ParkId"",
									'CreatedBy', st.""CreatedBy"",
									'CreatedAt', st.""CreatedAt"",
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
										WHERE p.""Id"" = st.""ParkId""
									)
								),
								'WasteInSolidState', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', wm.""Id"",
											'WasteId', wm.""WasteId"",
											'Name', wm.""Name"",
											'State', wm.""State"",
											'OtherName', wm.""OtherName"",
											'Quantity', wm.""Quantity"",
											'CreatedAt', wm.""CreatedAt"",
											'CreatedBy', wm.""CreatedBy"",
											'UpdatedAt', wm.""UpdatedAt"",
											'UpdatedBy', wm.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""WasteMaterials"" wm
									WHERE wm.""WasteId"" = w.""Id""
									AND wm.""State"" = 'Solid'
								),
								'WasteInLiquidState', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', wm.""Id"",
											'WasteId', wm.""WasteId"",
											'Name', wm.""Name"",
											'State', wm.""State"",
											'OtherName', wm.""OtherName"",
											'Quantity', wm.""Quantity"",
											'CreatedAt', wm.""CreatedAt"",
											'CreatedBy', wm.""CreatedBy"",
											'UpdatedAt', wm.""UpdatedAt"",
											'UpdatedBy', wm.""UpdatedBy""
										)
									), '[]'::jsonb)
									FROM ""WasteMaterials"" wm
									WHERE wm.""WasteId"" = w.""Id""
									AND wm.""State"" = 'Liquid'
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
						FROM ""Wastes"" w
						JOIN ""Stations"" st ON st.""Id"" = w.""StationId""
						JOIN ""Users"" u ON u.""Id"" = w.""CreatedBy""
						WHERE w.""Id"" = waste_id;
						
						-- data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_waste_by_id: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_waste(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
				    DECLARE
				        v_waste_id UUID;
						v_solid_state CONSTANT VARCHAR(50) := 'Solid';
						v_liquid_state CONSTANT VARCHAR(50) := 'Liquid';
				        v_solid_element JSONB;
						v_liquid_element JSONB;
						v_park_id UUID;
				    BEGIN  
						-- Get ParkId
						SELECT ""ParkId"" INTO v_park_id 
						FROM ""Stations""
						WHERE ""Id"" = (input_data->>'StationId')::UUID;
						
				        -- Insert Wastes data
				        BEGIN
				            INSERT INTO public.""Wastes""(
								""Id"", 
								""StationId"", 
								""ParkId"", 
								""Category"", 
								""LiquidStateRemark"", 
								""SolidStateRemark"", 
								""Coordinates"",
								""CreatedBy"",
								""CreatedAt""
				            ) VALUES (
				                gen_random_uuid(),
				                (input_data->>'StationId')::UUID,
								v_park_id,
								input_data->>'Category',
				                NULLIF(COALESCE(input_data->>'LiquidStateRemark', ''),''),
								NULLIF(COALESCE(input_data->>'SolidStateRemark', ''),''),
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
               					(input_data->>'CreatedBy')::UUID,
				                NOW()
				            ) RETURNING ""Id"" INTO v_waste_id;
				        EXCEPTION
				            WHEN OTHERS THEN
				                RAISE EXCEPTION 'Wastes table error: %', SQLERRM;
				        END;

				        -- Insert WasteMaterials Data
				        IF input_data->'LiquidStates' IS NOT NULL AND jsonb_array_length(input_data->'LiquidStates') > 0 THEN
				            FOR v_liquid_element IN
				                SELECT * FROM jsonb_array_elements(input_data->'LiquidStates')
				            LOOP
				                BEGIN
				                    INSERT INTO public.""WasteMaterials""(
										""Id"", 
										""WasteId"", 
										""Name"", 
										""State"",
										""OtherName"",
										""Quantity"",
				                        ""CreatedBy"",
				                        ""CreatedAt""
				                    ) VALUES (
				                        gen_random_uuid(),
										v_waste_id,
				                        v_liquid_element->>'Name',
										v_liquid_state,
										NULLIF(COALESCE(v_liquid_element->>'OtherName', ''),''),
										(v_liquid_element->>'Quantity')::NUMERIC,
				                        (input_data->>'CreatedBy')::UUID,
				                        NOW()
				                    );
				                EXCEPTION
				                    WHEN OTHERS THEN
				                        RAISE EXCEPTION 'WasteMaterials (LiquidStates) table error: %', SQLERRM;
				                END;
				            END LOOP;
				        END IF;

						-- Insert WasteMaterials Data
				        IF input_data->'SolidStates' IS NOT NULL AND jsonb_array_length(input_data->'SolidStates') > 0 THEN
				            FOR v_solid_element IN
				                SELECT * FROM jsonb_array_elements(input_data->'SolidStates')
				            LOOP
				                BEGIN
				                    INSERT INTO public.""WasteMaterials""(
										""Id"", 
										""WasteId"", 
										""Name"", 
										""State"",
										""OtherName"",
										""Quantity"",
				                        ""CreatedBy"",
				                        ""CreatedAt""
				                    ) VALUES (
				                        gen_random_uuid(),
										v_waste_id,
				                        v_solid_element->>'Name',
										v_solid_state,
										NULLIF(COALESCE(v_solid_element->>'OtherName', ''),''),
										(v_solid_element->>'Quantity')::NUMERIC,
				                        (input_data->>'CreatedBy')::UUID,
				                        NOW()
				                    );
				                EXCEPTION
				                    WHEN OTHERS THEN
				                        RAISE EXCEPTION 'WasteMaterials (SolidStates) table error: %', SQLERRM;
				                END;
				            END LOOP;
				        END IF;
				        
				        -- Return the created v_waste_id
				        RETURN v_waste_id::TEXT;
				        
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'Error in fn_create_waste: %', SQLERRM;
				    END;        
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_waste(
					input_data jsonb,
					waste_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE
				    v_waste_id UUID;
					v_solid_state CONSTANT VARCHAR(50) := 'Solid';
					v_liquid_state CONSTANT VARCHAR(50) := 'Liquid';
					v_solid_element JSONB;
					v_liquid_element JSONB;
					v_park_id UUID;
				BEGIN 
				    -- Update Wastes data
				    BEGIN
				        -- Convert v_waste_id parameter to UUID
				        v_waste_id := waste_id::UUID;

						-- Get ParkId
						SELECT ""ParkId"" INTO v_park_id 
						FROM ""Stations""
						WHERE ""Id"" = (input_data->>'StationId')::UUID;
				        
				        UPDATE public.""Wastes""
				        SET
				            ""StationId"" = (input_data->>'StationId')::UUID,
							""ParkId"" = v_park_id,
							""Category"" = input_data->>'Category',
							""LiquidStateRemark"" = NULLIF(COALESCE(input_data->>'LiquidStateRemark', ''),''),
							""SolidStateRemark"" = NULLIF(COALESCE(input_data->>'SolidStateRemark', ''),''),
				            ""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
				            ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
				            ""UpdatedAt"" = NOW()
				        WHERE ""Id"" = v_waste_id
				        RETURNING ""Id"" INTO v_waste_id;
				    EXCEPTION
				        WHEN OTHERS THEN
				            RAISE EXCEPTION 'Wastes table error: %', SQLERRM;
				    END;

				    -- Update LiquidStates Data
				    BEGIN
				        IF input_data->'LiquidStates' IS NOT NULL AND jsonb_array_length(input_data->'LiquidStates') > 0 THEN
				            -- Delete existing WasteMaterials
				            DELETE FROM public.""WasteMaterials""
				            WHERE ""WasteId"" = v_waste_id
							AND ""State"" = v_liquid_state;
				                
				            -- Insert WasteMaterials data	
				            FOR v_liquid_element IN
				                SELECT * FROM jsonb_array_elements(input_data->'LiquidStates')
				            LOOP
				                INSERT INTO public.""WasteMaterials""(
									""Id"", 
									""WasteId"", 
									""Name"", 
									""State"",
									""OtherName"",
									""Quantity"",
				                    ""CreatedBy"",
				                    ""CreatedAt"",
				                    ""UpdatedBy"",
				                    ""UpdatedAt""
				                ) VALUES (
				                    gen_random_uuid(),
									v_waste_id,
									v_liquid_element->>'Name',
									v_liquid_state,
									NULLIF(COALESCE(v_liquid_element->>'OtherName', ''),''),
									(v_liquid_element->>'Quantity')::NUMERIC,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
						ELSE
							-- Only delete if there are existing WasteMaterials
							IF EXISTS (
								SELECT 1 FROM public.""WasteMaterials"" 
								WHERE ""WasteId"" = v_waste_id
								AND ""State"" = v_liquid_state
							) THEN
								DELETE FROM public.""WasteMaterials""
								WHERE ""WasteId"" = v_waste_id 
								AND ""State"" = v_liquid_state;
							END IF;
						END IF;
						-- EXCEPTION HANDLING
					    EXCEPTION
					        WHEN OTHERS THEN
					            RAISE EXCEPTION 'WasteMaterials (LiquidStates) table error: %', SQLERRM;
				    END;

					-- Update SolidStates Data
					BEGIN
				        IF input_data->'SolidStates' IS NOT NULL AND jsonb_array_length(input_data->'SolidStates') > 0 THEN
				            -- Delete existing WasteMaterials
				            DELETE FROM public.""WasteMaterials""
				            WHERE ""WasteId"" = v_waste_id
							AND ""State"" = v_solid_state;
				                
				            -- Insert WasteMaterials data	
				            FOR v_solid_element IN
				                SELECT * FROM jsonb_array_elements(input_data->'SolidStates')
				            LOOP
				                INSERT INTO public.""WasteMaterials""(
									""Id"", 
									""WasteId"", 
									""Name"", 
									""State"",
									""OtherName"",
									""Quantity"",
				                    ""CreatedBy"",
				                    ""CreatedAt"",
				                    ""UpdatedBy"",
				                    ""UpdatedAt""
				                ) VALUES (
				                    gen_random_uuid(),
									v_waste_id,
									v_solid_element->>'Name',
									v_solid_state,
									NULLIF(COALESCE(v_solid_element->>'OtherName', ''),''),
									(v_solid_element->>'Quantity')::NUMERIC,
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW(),
				                    (input_data->>'UpdatedBy')::UUID,
				                    NOW()
				                );
				            END LOOP;
						ELSE
							-- Only delete if there are existing WasteMaterials
							IF EXISTS (
								SELECT 1 FROM public.""WasteMaterials"" 
								WHERE ""WasteId"" = v_waste_id
								AND ""State"" = v_solid_state
							) THEN
								DELETE FROM public.""WasteMaterials""
								WHERE ""WasteId"" = v_waste_id 
								AND ""State"" = v_solid_state;
							END IF;
						END IF;
						-- EXCEPTION HANDLING
					    EXCEPTION
					        WHEN OTHERS THEN
					            RAISE EXCEPTION 'WasteMaterials (SolidStates) table error: %', SQLERRM;
				    END;

				    -- Return the updated v_waste_id
				    RETURN v_waste_id::TEXT;
				    
				EXCEPTION
				    WHEN OTHERS THEN
				        RAISE EXCEPTION 'Error in fn_update_waste: %', SQLERRM;
				END;        
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_wastes(integer, integer, character varying, character varying, character varying, uuid[]);
				DROP FUNCTION IF EXISTS public.fn_waste_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_create_waste(jsonb);
				DROP FUNCTION IF EXISTS public.fn_update_waste(jsonb, character varying);
            ");
        }
    }
}
