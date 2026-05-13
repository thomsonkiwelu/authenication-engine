using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMangabeyObservationFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_mangabey_observation(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_mangabey_observation_id UUID;
						v_mating_element JSONB;
						v_fighting_element JSONB;
						v_other_animal_element JSONB;
					BEGIN  
						-- Insert MangabeyObservations data
						BEGIN
							INSERT INTO public.""MangabeyObservations""(
								""Id"",
								""ParkId"", 
								""ActivityType"", 
								""NumberOfParticipant"", 
								""Coordinates"",
								""IsGroupTypeLetu"",
								""IsGroupTypeLingine"",
								""CreatedBy"",
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'ParkId')::UUID,
								input_data->>'ActivityType',
								(input_data->>'NumberOfParticipant')::NUMERIC,
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								COALESCE((input_data->>'IsGroupTypeLetu')::BOOLEAN, FALSE),
								COALESCE((input_data->>'IsGroupTypeLingine')::BOOLEAN, FALSE),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_mangabey_observation_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'MangabeyObservations table error: %', SQLERRM;
						END;

						-- Insert MangabeyEatingBehaviors Data
						IF input_data->>'ActivityType' = 'eating' THEN
							BEGIN
								INSERT INTO public.""MangabeyEatingBehaviors""(
									""Id"", 
									""SpeciesId"", 
									""ConsumedTreePart"", 
									""OtherFood"", 
									""OtherInsect"", 
									""MangabeyObservationId"",
									""CreatedBy"",
									""CreatedAt""
								) VALUES (
									gen_random_uuid(),
									(input_data->>'SpeciesId')::UUID,
									input_data->>'ConsumedTreePart',
									input_data->>'OtherFood',
									NULLIF(COALESCE(input_data->>'OtherInsect', ''),''),
									v_mangabey_observation_id,
									(input_data->>'CreatedBy')::UUID,
									NOW()
								);
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'MangabeyEatingBehaviors table error: %', SQLERRM;
							END;
							
						-- Insert MangabeyEatingBehaviors Data	
						ELSIF input_data->>'ActivityType' = 'mating' THEN
						
							IF input_data->'MatingData' IS NOT NULL AND jsonb_array_length(input_data->'MatingData') > 0 THEN
								FOR v_mating_element IN
									SELECT * FROM jsonb_array_elements(input_data->'MatingData')
								LOOP
									BEGIN
										INSERT INTO public.""MangabeyMatingBehaviors""(
											""Id"", 
											""MaleMating"", 
											""FemaleMating"", 
											""WhatHappened"", 
											""MangabeyObservationId"",
											""CreatedBy"",
											""CreatedAt""
										) VALUES (
											gen_random_uuid(),
											v_mating_element->>'MaleMating',
											v_mating_element->>'FemaleMating',
											v_mating_element->>'WhatHappened',
											v_mangabey_observation_id,
											(input_data->>'CreatedBy')::UUID,
											NOW()
										);
									EXCEPTION
										WHEN OTHERS THEN
											RAISE EXCEPTION 'MangabeyMatingBehaviors table error: %', SQLERRM;
									END;
								END LOOP;
							END IF;

						-- Insert MangabeyFightingBehaviors Data	
						ELSIF input_data->>'ActivityType' = 'fighting' THEN
						
							IF input_data->'FightingData' IS NOT NULL AND jsonb_array_length(input_data->'FightingData') > 0 THEN
								FOR v_fighting_element IN
									SELECT * FROM jsonb_array_elements(input_data->'FightingData')
								LOOP
									BEGIN
										INSERT INTO public.""MangabeyFightingBehaviors""(
											""Id"", 
											""AggressiveIndividual"", 
											""AttackedIndividual"", 
											""WhatHappened"", 
											""MangabeyObservationId"",
											""CreatedBy"",
											""CreatedAt""
										) VALUES (
											gen_random_uuid(),
											v_fighting_element->>'AggressiveIndividual',
											v_fighting_element->>'AttackedIndividual',
											v_fighting_element->>'WhatHappened',
											v_mangabey_observation_id,
											(input_data->>'CreatedBy')::UUID,
											NOW()
										);
									EXCEPTION
										WHEN OTHERS THEN
											RAISE EXCEPTION 'MangabeyFightingBehaviors table error: %', SQLERRM;
									END;
								END LOOP;
							END IF;

						-- Insert MangabeyOtherSpecieObservations Data	
						ELSIF input_data->>'ActivityType' = 'other_animals' THEN
						
							IF input_data->'OtherAnimalData' IS NOT NULL AND jsonb_array_length(input_data->'OtherAnimalData') > 0 THEN
								FOR v_other_animal_element IN
									SELECT * FROM jsonb_array_elements(input_data->'OtherAnimalData')
								LOOP
									BEGIN
										INSERT INTO public.""MangabeyOtherSpecieObservations""( 
											""Id"", 
											""SpeciesId"", 
											""ActivityObserved"", 
											""NumberOfAnimalSeen"",
											""MangabeyBehavior"", 
											""MangabeyObservationId"",
											""CreatedBy"",
											""CreatedAt""
										) VALUES (
											gen_random_uuid(),
											(v_other_animal_element->>'SpeciesId')::UUID,
											v_other_animal_element->>'ActivityObserved',
											v_other_animal_element->>'NumberOfAnimalSeen',
											v_other_animal_element->>'MangabeyBehavior',
											v_mangabey_observation_id,
											(input_data->>'CreatedBy')::UUID,
											NOW()
										);
									EXCEPTION
										WHEN OTHERS THEN
											RAISE EXCEPTION 'MangabeyOtherSpecieObservations table error: %', SQLERRM;
									END;
								END LOOP;
							END IF;
							
						END IF;
						
						-- Return the created v_mangabey_observation_id
						RETURN v_mangabey_observation_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_mangabey_observation: %', SQLERRM;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_mangabey_observation_by_id(
				    mangabey_observation_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
					    response_data JSONB;
					    v_activity_type TEXT;
					BEGIN
					    -- First get the activity type
					    SELECT mo.""ActivityType"" INTO v_activity_type
					    FROM ""MangabeyObservations"" mo
					    WHERE mo.""Id"" = mangabey_observation_id;
				    
					    -- Build Response
					    SELECT COALESCE(
					        jsonb_build_object(
					            'Id', mo.""Id"",
					            'ParkId', mo.""ParkId"", 
					            'ActivityType', mo.""ActivityType"",
					            'NumberOfParticipant', mo.""NumberOfParticipant"",
					            'Coordinates', mo.""Coordinates"",
					            'IsGroupTypeLetu', mo.""IsGroupTypeLetu"",
					            'IsGroupTypeLingine', mo.""IsGroupTypeLingine"",    
					            'CreatedBy', mo.""CreatedBy"",
					            'CreatedAt', mo.""CreatedAt"",
					            'UpdatedBy', mo.""UpdatedBy"",
					            'UpdatedAt', mo.""UpdatedAt"",
					            'Park', jsonb_build_object(
					                'Id', p.""Id"",
					                'Name', p.""Name"",
					                'Code', p.""Code"",
					                'Zone', p.""Zone"",
					                'CreatedAt', p.""CreatedAt"",
					                'CreatedBy', p.""CreatedBy""
					            ),
					            -- MangabeyEatingBehavior
					            'MangabeyEatingBehavior', 
					            CASE 
					                WHEN v_activity_type = 'eating' THEN
					                    (SELECT COALESCE(jsonb_build_object(
					                        'Id', meb.""Id"",
					                        'SpeciesId', meb.""SpeciesId"",
					                        'ConsumedTreePart', meb.""ConsumedTreePart"",
					                        'OtherFood', meb.""OtherFood"",
					                        'OtherInsect', meb.""OtherInsect"",
											'MangabeyObservationId', meb.""MangabeyObservationId"",
											'CreatedAt', meb.""CreatedAt"",
	                						'CreatedBy', meb.""CreatedBy"",
					                        'Species', jsonb_build_object(
					                            'Id', s.""Id"",
					                            'CommonName', s.""CommonName"",
					                            'ScientificName', s.""ScientificName"",
												'Type', s.""Type"",
												'CreatedAt', s.""CreatedAt"",
	                							'CreatedBy', s.""CreatedBy""
					                        )
					                    ), '{}'::JSONB)
					                    FROM ""MangabeyEatingBehaviors"" meb
					                    JOIN ""Species"" s ON s.""Id"" = meb.""SpeciesId""
					                    WHERE meb.""MangabeyObservationId"" = mo.""Id"")
					                ELSE '{}'::JSONB
					            END,
					            
					            -- MangabeyMatingBehaviors
					            'MangabeyMatingBehaviors',
					            CASE 
					                WHEN v_activity_type = 'mating' THEN
					                    (SELECT COALESCE(jsonb_agg(
					                        jsonb_build_object(
					                            'Id', m.""Id"",
					                            'MaleMating', m.""MaleMating"",
					                            'FemaleMating', m.""FemaleMating"",
					                            'WhatHappened', m.""WhatHappened"",
												'MangabeyObservationId', m.""MangabeyObservationId"",
					                            'CreatedAt', m.""CreatedAt"",
					                            'CreatedBy', m.""CreatedBy""
					                        )
					                    ), '[]'::JSONB)
					                    FROM ""MangabeyMatingBehaviors"" m
					                    WHERE m.""MangabeyObservationId"" = mo.""Id"")
					                ELSE '[]'::JSONB
					            END,
					            
					            -- MangabeyFightingBehaviors
					            'MangabeyFightingBehaviors', 
					            CASE 
					                WHEN v_activity_type = 'fighting' THEN
					                    (SELECT COALESCE(jsonb_agg(
					                        jsonb_build_object(
					                            'Id', f.""Id"",
					                            'AggressiveIndividual', f.""AggressiveIndividual"",
					                            'AttackedIndividual', f.""AttackedIndividual"",
					                            'WhatHappened', f.""WhatHappened"",
												'MangabeyObservationId', f.""MangabeyObservationId"",
					                            'CreatedAt', f.""CreatedAt"",
					                            'CreatedBy', f.""CreatedBy""
					                        )
					                    ), '[]'::JSONB)
					                    FROM ""MangabeyFightingBehaviors"" f
					                    WHERE f.""MangabeyObservationId"" = mo.""Id"")
					                ELSE '[]'::JSONB
					            END,
					            
					            -- MangabeyOtherSpecieObservations
					            'MangabeyOtherSpecieObservations', 
					            CASE 
					                WHEN v_activity_type = 'other_animals' THEN
					                    (SELECT COALESCE(jsonb_agg(
					                        jsonb_build_object(
					                            'Id', o.""Id"",
					                            'SpeciesId', o.""SpeciesId"",
					                            'ActivityObserved', o.""ActivityObserved"",
					                            'NumberOfAnimalSeen', o.""NumberOfAnimalSeen"",
					                            'MangabeyBehavior', o.""MangabeyBehavior"",
												'MangabeyObservationId', o.""MangabeyObservationId"",
												'CreatedAt', o.""CreatedAt"",
	                							'CreatedBy', o.""CreatedBy"",
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
					                    FROM ""MangabeyOtherSpecieObservations"" o
					                    JOIN ""Species"" s ON s.""Id"" = o.""SpeciesId""
					                    WHERE o.""MangabeyObservationId"" = mo.""Id"")
					                ELSE '[]'::JSONB
					            END,
					            
					            'CreatedByUser', jsonb_build_object(
					                'Id', u.""Id"",
					                'Username', u.""Username"",
					                'Email', u.""Email""
					            ),
							    'UpdatedByUser', (
									SELECT jsonb_build_object(
										'Id', upd.""Id"",
										'Username', upd.""Username"",
										'Email', upd.""Email""
									)
									FROM ""Users"" upd
									WHERE upd.""Id"" = mo.""UpdatedBy""
								)
					        ),
					        '{}'::JSONB
					    ) INTO response_data
					    FROM ""MangabeyObservations"" mo
					    JOIN ""Parks"" p ON p.""Id"" = mo.""ParkId""
					    JOIN ""Users"" u ON u.""Id"" = mo.""CreatedBy""
					    WHERE mo.""Id"" = mangabey_observation_id;
					    
					    -- Return the response
					    RETURN COALESCE(response_data, '{}'::JSONB);
				    
					EXCEPTION
					    WHEN OTHERS THEN
					        RAISE EXCEPTION 'Error in fn_mangabey_observation_by_id: %', SQLERRM;
					END;    
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_mangabey_observation(
					input_data jsonb,
					mangabey_observation_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_mangabey_observation_id UUID;
						v_mating_element JSONB;
						v_fighting_element JSONB;
						v_other_animal_element JSONB;
					BEGIN 
						-- Update MangabeyObservations data
						BEGIN
							-- Convert v_mangabey_observation_id parameter to UUID
							v_mangabey_observation_id := mangabey_observation_id::UUID;
							
							UPDATE public.""MangabeyObservations""
							SET
								""ParkId"" = (input_data->>'ParkId')::UUID,
								""ActivityType"" = input_data->>'ActivityType',
								""NumberOfParticipant"" = (input_data->>'NumberOfParticipant')::NUMERIC,
								""IsGroupTypeLetu"" = COALESCE((input_data->>'IsGroupTypeLetu')::BOOLEAN, FALSE),
								""IsGroupTypeLingine"" = COALESCE((input_data->>'IsGroupTypeLingine')::BOOLEAN, FALSE),
								""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_mangabey_observation_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'MangabeyObservations table error: %', SQLERRM;
						END;

						-- Update MangabeyEatingBehaviors Data
						IF input_data->>'ActivityType' = 'eating' THEN
						    BEGIN
						        IF EXISTS (SELECT 1 FROM public.""MangabeyEatingBehaviors"" WHERE ""MangabeyObservationId"" = v_mangabey_observation_id) THEN
						            UPDATE public.""MangabeyEatingBehaviors""
						            SET
						                ""SpeciesId"" = (input_data->>'SpeciesId')::UUID,
						                ""ConsumedTreePart"" = input_data->>'ConsumedTreePart',
						                ""OtherFood"" = input_data->>'OtherFood',
						                ""OtherInsect"" = NULLIF(TRIM(COALESCE(input_data->>'OtherInsect', '')), ''),
						                ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
						                ""UpdatedAt"" = NOW()
						            WHERE ""MangabeyObservationId"" = v_mangabey_observation_id;
						        ELSE
						            INSERT INTO public.""MangabeyEatingBehaviors""
									(
						                ""Id"", 
										""SpeciesId"", 
										""ConsumedTreePart"", 
										""OtherFood"", 
										""OtherInsect"", 
						                ""MangabeyObservationId"", 
										""CreatedBy"", 
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
						            ) VALUES (
						                gen_random_uuid(),
						                (input_data->>'SpeciesId')::UUID,
						                input_data->>'ConsumedTreePart',
						                input_data->>'OtherFood',
						                NULLIF(COALESCE(input_data->>'OtherInsect', ''), ''),
						                v_mangabey_observation_id,
						                (input_data->>'UpdatedBy')::UUID,
						                NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
						            );
						        END IF;
						    EXCEPTION
						        WHEN OTHERS THEN
						            RAISE EXCEPTION 'MangabeyEatingBehaviors table error: %', SQLERRM;
						    END;
						ELSE
						    -- Delete if exists
						    DELETE FROM public.""MangabeyEatingBehaviors""
						    WHERE ""MangabeyObservationId"" = v_mangabey_observation_id;
						END IF;

						-- Update MangabeyMatingBehaviors Data
						IF input_data->>'ActivityType' = 'mating' THEN
							IF input_data->'MatingData' IS NOT NULL AND jsonb_array_length(input_data->'MatingData') > 0 THEN
								-- Delete if exists
								DELETE FROM public.""MangabeyMatingBehaviors""
								WHERE ""MangabeyObservationId"" = v_mangabey_observation_id;
											
								FOR v_mating_element IN
									SELECT * FROM jsonb_array_elements(input_data->'MatingData')
								LOOP
									BEGIN
										INSERT INTO public.""MangabeyMatingBehaviors""(
											""Id"", 
											""MaleMating"", 
											""FemaleMating"", 
											""WhatHappened"", 
											""MangabeyObservationId"",
											""CreatedBy"",
											""CreatedAt"",
											""UpdatedBy"",
											""UpdatedAt""
										) VALUES (
											gen_random_uuid(),
											v_mating_element->>'MaleMating',
											v_mating_element->>'FemaleMating',
											v_mating_element->>'WhatHappened',
											v_mangabey_observation_id,
											(input_data->>'UpdatedBy')::UUID,
											NOW(),
											(input_data->>'UpdatedBy')::UUID,
											NOW()
										);
									EXCEPTION
										WHEN OTHERS THEN
											RAISE EXCEPTION 'MangabeyMatingBehaviors table error: %', SQLERRM;
									END;
								END LOOP;
							END IF;
						ELSE
							IF EXISTS (SELECT 1 FROM public.""MangabeyMatingBehaviors"" WHERE ""MangabeyObservationId"" = v_mangabey_observation_id) THEN
					            DELETE FROM public.""MangabeyMatingBehaviors""
					            WHERE ""MangabeyObservationId"" = v_mangabey_observation_id;
					        END IF;
						END IF;
						
						-- Update MangabeyFightingBehaviors Data
						IF input_data->>'ActivityType' = 'fighting' THEN
						
							IF input_data->'FightingData' IS NOT NULL AND jsonb_array_length(input_data->'FightingData') > 0 THEN
							
								-- Delete existing MangabeyFightingBehaviors
								DELETE FROM public.""MangabeyFightingBehaviors""
								WHERE ""MangabeyObservationId"" = v_mangabey_observation_id;
								
								FOR v_fighting_element IN
									SELECT * FROM jsonb_array_elements(input_data->'FightingData')
								LOOP
									BEGIN
										INSERT INTO public.""MangabeyFightingBehaviors""(
											""Id"", 
											""AggressiveIndividual"", 
											""AttackedIndividual"", 
											""WhatHappened"", 
											""MangabeyObservationId"",
											""CreatedBy"",
											""CreatedAt"",
											""UpdatedBy"",
											""UpdatedAt""
										) VALUES (
											gen_random_uuid(),
											v_fighting_element->>'AggressiveIndividual',
											v_fighting_element->>'AttackedIndividual',
											v_fighting_element->>'WhatHappened',
											v_mangabey_observation_id,
											(input_data->>'UpdatedBy')::UUID,
											NOW(),
											(input_data->>'UpdatedBy')::UUID,
											NOW()
										);
									EXCEPTION
										WHEN OTHERS THEN
											RAISE EXCEPTION 'MangabeyFightingBehaviors table error: %', SQLERRM;
									END;
								END LOOP;
							END IF;
						ELSE
							IF EXISTS (SELECT 1 FROM public.""MangabeyFightingBehaviors"" WHERE ""MangabeyObservationId"" = v_mangabey_observation_id) THEN
					            DELETE FROM public.""MangabeyFightingBehaviors""
					            WHERE ""MangabeyObservationId"" = v_mangabey_observation_id;
					        END IF;
						END IF;		

						-- Update MangabeyOtherSpecieObservations Data	
						IF input_data->>'ActivityType' = 'other_animals' THEN
							IF input_data->'OtherAnimalData' IS NOT NULL AND jsonb_array_length(input_data->'OtherAnimalData') > 0 THEN
								-- Delete
								DELETE FROM public.""MangabeyOtherSpecieObservations""
								WHERE ""MangabeyObservationId"" = v_mangabey_observation_id;
								
								FOR v_other_animal_element IN
									SELECT * FROM jsonb_array_elements(input_data->'OtherAnimalData')
								LOOP
									BEGIN
										INSERT INTO public.""MangabeyOtherSpecieObservations""( 
											""Id"", 
											""SpeciesId"", 
											""ActivityObserved"", 
											""NumberOfAnimalSeen"",
											""MangabeyBehavior"", 
											""MangabeyObservationId"",
											""CreatedBy"",
											""CreatedAt"",
											""UpdatedBy"",
											""UpdatedAt""
										) VALUES (
											gen_random_uuid(),
											(v_other_animal_element->>'SpeciesId')::UUID,
											v_other_animal_element->>'ActivityObserved',
											v_other_animal_element->>'NumberOfAnimalSeen',
											v_other_animal_element->>'MangabeyBehavior',
											v_mangabey_observation_id,
											(input_data->>'UpdatedBy')::UUID,
											NOW(),
											(input_data->>'UpdatedBy')::UUID,
											NOW()
										);
									EXCEPTION
										WHEN OTHERS THEN
											RAISE EXCEPTION 'MangabeyOtherSpecieObservations table error: %', SQLERRM;
									END;
								END LOOP;
							END IF;
						ELSE
							IF EXISTS (SELECT 1 FROM public.""MangabeyOtherSpecieObservations"" WHERE ""MangabeyObservationId"" = v_mangabey_observation_id) THEN
					            DELETE FROM public.""MangabeyOtherSpecieObservations""
					            WHERE ""MangabeyObservationId"" = v_mangabey_observation_id;
					        END IF;
						END IF;

						-- Return the updated v_mangabey_observation_id
						RETURN v_mangabey_observation_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_mangabey_observation: %', SQLERRM;
					END;		
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_create_mangabey_observation(jsonb);
				DROP FUNCTION IF EXISTS public.fn_mangabey_observation_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_mangabey_observation(jsonb, character varying);
            ");
        }
    }
}
