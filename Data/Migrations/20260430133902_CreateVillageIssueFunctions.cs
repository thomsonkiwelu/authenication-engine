using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateVillageIssueFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_village_issues(
					input_data jsonb
				)
				    RETURNS boolean
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_array_element TEXT;
					BEGIN
						-- Insert ParkRelatedProblems Data
						IF input_data->'ParkRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'ParkRelatedProblems') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'ParkRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"",
										""Value"",
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'ParkRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherParkRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (ParkRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert landRelatedProblems Data
						IF input_data->'LandRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'LandRelatedProblems') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'LandRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'LandRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherLandRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (LandRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert animalHusbandryRelatedProblems Data
						IF input_data->'AnimalHusbandryRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'AnimalHusbandryRelatedProblems') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'AnimalHusbandryRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'AnimalHusbandryRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherAnimalHusbandryRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (AnimalHusbandryRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert foodSupplyAndSecurityProblems Data
						IF input_data->'FoodSupplyAndSecurityProblems' IS NOT NULL AND jsonb_array_length(input_data->'FoodSupplyAndSecurityProblems') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'FoodSupplyAndSecurityProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'FoodSupplyAndSecurityProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherFoodSupplyAndSecurityProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (FoodSupplyAndSecurityProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert securityRelatedProblems Data
						IF input_data->'SecurityRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'SecurityRelatedProblems') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'SecurityRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'SecurityRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherSecurityRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (SecurityRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert leadershipRelatedProblems Data
						IF input_data->'LeadershipRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'LeadershipRelatedProblems') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'LeadershipRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'LeadershipRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherLeadershipRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (LeadershipRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return success
						RETURN true;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_village_issues: %', SQLERRM;
							RETURN false;
					END;		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_village_issues(
					input_data jsonb
				)
				    RETURNS boolean
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_array_element TEXT;
						v_entity_name CONSTANT VARCHAR(50) := 'VillageProfile';
					BEGIN
						-- Insert ParkRelatedProblems Data
						IF input_data->'ParkRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'ParkRelatedProblems') > 0 THEN

							-- Delete existing ParkRelatedProblems
							DELETE FROM public.""CommunitySelections""
							WHERE ""EntityId"" = (input_data->>'EntityId')::UUID
							AND ""EntityName"" = v_entity_name
							AND ""FieldName"" = 'ParkRelatedProblems';
											
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'ParkRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"",
										""Value"",
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'ParkRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherParkRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (ParkRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert landRelatedProblems Data
						IF input_data->'LandRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'LandRelatedProblems') > 0 THEN

							-- Delete existing LandRelatedProblems
							DELETE FROM public.""CommunitySelections""
							WHERE ""EntityId"" = (input_data->>'EntityId')::UUID
							AND ""EntityName"" = v_entity_name
							AND ""FieldName"" = 'LandRelatedProblems';
							
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'LandRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'LandRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherLandRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (LandRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert animalHusbandryRelatedProblems Data
						IF input_data->'AnimalHusbandryRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'AnimalHusbandryRelatedProblems') > 0 THEN
							
							-- Delete existing AnimalHusbandryRelatedProblems
							DELETE FROM public.""CommunitySelections""
							WHERE ""EntityId"" = (input_data->>'EntityId')::UUID
							AND ""EntityName"" = v_entity_name
							AND ""FieldName"" = 'AnimalHusbandryRelatedProblems';
							
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'AnimalHusbandryRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'AnimalHusbandryRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherAnimalHusbandryRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (AnimalHusbandryRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert foodSupplyAndSecurityProblems Data
						IF input_data->'FoodSupplyAndSecurityProblems' IS NOT NULL AND jsonb_array_length(input_data->'FoodSupplyAndSecurityProblems') > 0 THEN
							
							-- Delete existing FoodSupplyAndSecurityProblems
							DELETE FROM public.""CommunitySelections""
							WHERE ""EntityId"" = (input_data->>'EntityId')::UUID
							AND ""EntityName"" = v_entity_name
							AND ""FieldName"" = 'FoodSupplyAndSecurityProblems';
							
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'FoodSupplyAndSecurityProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'FoodSupplyAndSecurityProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherFoodSupplyAndSecurityProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (FoodSupplyAndSecurityProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert securityRelatedProblems Data
						IF input_data->'SecurityRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'SecurityRelatedProblems') > 0 THEN
							
							-- Delete existing SecurityRelatedProblems
							DELETE FROM public.""CommunitySelections""
							WHERE ""EntityId"" = (input_data->>'EntityId')::UUID
							AND ""EntityName"" = v_entity_name
							AND ""FieldName"" = 'SecurityRelatedProblems';
							
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'SecurityRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'SecurityRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherSecurityRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (SecurityRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert leadershipRelatedProblems Data
						IF input_data->'LeadershipRelatedProblems' IS NOT NULL AND jsonb_array_length(input_data->'LeadershipRelatedProblems') > 0 THEN
							
							-- Delete existing LeadershipRelatedProblems
							DELETE FROM public.""CommunitySelections""
							WHERE ""EntityId"" = (input_data->>'EntityId')::UUID 
							AND ""EntityName"" = v_entity_name
							AND ""FieldName"" = 'LeadershipRelatedProblems';
							
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'LeadershipRelatedProblems')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
										""Id"", 
										""Value"", 
										""FieldName"",
										""OtherName"",
										""EntityId"",
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_array_element,
										'LeadershipRelatedProblems',
										CASE WHEN v_array_element = 'other'
						                    THEN NULLIF(COALESCE(input_data->>'OtherLeadershipRelatedProblem', ''), '')
						                    ELSE NULL
						                END,
										(input_data->>'EntityId')::UUID,
										input_data->>'EntityName',
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (LeadershipRelatedProblems) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return success
						RETURN true;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_village_issues: %', SQLERRM;
							RETURN false;
					END;  		
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_village_issues_by_id(
					village_profile_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
						    jsonb_build_object(
						        -- Park Related Problems
						        'ParkRelatedProblems', (
						            SELECT COALESCE(jsonb_agg(
						                jsonb_build_object(
						                    'Id', cs.""Id"",
						                    'Value', cs.""Value"",
						                    'OtherName', cs.""OtherName"",
						                    'FieldName', cs.""FieldName"",
											'EntityId', cs.""EntityId"",
						                    'EntityName', cs.""EntityName"",
						                    'CreatedAt', cs.""CreatedAt"",
						                    'CreatedBy', cs.""CreatedBy""
						                )
						            ), '[]'::jsonb)
						            FROM ""CommunitySelections"" cs
						            WHERE cs.""EntityId"" = village_profile_id 
						                AND cs.""FieldName"" = 'ParkRelatedProblems'
						        ),
						        
						        -- Land Related Problems
						        'LandRelatedProblems', (
						            SELECT COALESCE(jsonb_agg(
						                jsonb_build_object(
						                    'Id', cs.""Id"",
						                    'Value', cs.""Value"",
						                    'OtherName', cs.""OtherName"",
											'EntityId', cs.""EntityId"",
						                    'EntityName', cs.""EntityName"",
						                    'CreatedAt', cs.""CreatedAt"",
						                    'CreatedBy', cs.""CreatedBy""
						                )
						            ), '[]'::jsonb)
						            FROM ""CommunitySelections"" cs
						            WHERE cs.""EntityId"" = village_profile_id 
						                AND cs.""FieldName"" = 'LandRelatedProblems'
						        ),
						        
						        -- Animal Husbandry Related Problems
						        'AnimalHusbandryRelatedProblems', (
						            SELECT COALESCE(jsonb_agg(
						                jsonb_build_object(
						                    'Id', cs.""Id"",
						                    'Value', cs.""Value"",
						                    'OtherName', cs.""OtherName"",
											'EntityId', cs.""EntityId"",
						                    'EntityName', cs.""EntityName"",
						                    'CreatedAt', cs.""CreatedAt"",
						                    'CreatedBy', cs.""CreatedBy""
						                )
						            ), '[]'::jsonb)
						            FROM ""CommunitySelections"" cs
						            WHERE cs.""EntityId"" = village_profile_id 
						                AND cs.""FieldName"" = 'AnimalHusbandryRelatedProblems'
						        ),
						        
						        -- Food Supply and Security Problems
						        'FoodSupplyAndSecurityProblems', (
						            SELECT COALESCE(jsonb_agg(
						                jsonb_build_object(
						                    'Id', cs.""Id"",
						                    'Value', cs.""Value"",
						                    'OtherName', cs.""OtherName"",
											'EntityId', cs.""EntityId"",
						                    'EntityName', cs.""EntityName"",
						                    'CreatedAt', cs.""CreatedAt"",
						                    'CreatedBy', cs.""CreatedBy""
						                )
						            ), '[]'::jsonb)
						            FROM ""CommunitySelections"" cs
						            WHERE cs.""EntityId"" = village_profile_id 
						                AND cs.""FieldName"" = 'FoodSupplyAndSecurityProblems'
						        ),
						        
						        -- Security Related Problems
						        'SecurityRelatedProblems', (
						            SELECT COALESCE(jsonb_agg(
						                jsonb_build_object(
						                    'Id', cs.""Id"",
						                    'Value', cs.""Value"",
						                    'OtherName', cs.""OtherName"",
											'EntityId', cs.""EntityId"",
						                    'EntityName', cs.""EntityName"",
						                    'CreatedAt', cs.""CreatedAt"",
						                    'CreatedBy', cs.""CreatedBy""
						                )
						            ), '[]'::jsonb)
						            FROM ""CommunitySelections"" cs
						            WHERE cs.""EntityId"" = village_profile_id 
						                AND cs.""FieldName"" = 'SecurityRelatedProblems'
						        ),
						        
						        -- Leadership Related Problems
						        'LeadershipRelatedProblems', (
						            SELECT COALESCE(jsonb_agg(
						                jsonb_build_object(
						                    'Id', cs.""Id"",
						                    'Value', cs.""Value"",
						                    'OtherName', cs.""OtherName"",
											'EntityId', cs.""EntityId"",
						                    'EntityName', cs.""EntityName"",
						                    'CreatedAt', cs.""CreatedAt"",
						                    'CreatedBy', cs.""CreatedBy""
						                )
						            ), '[]'::jsonb)
						            FROM ""CommunitySelections"" cs
						            WHERE cs.""EntityId"" = village_profile_id 
						                AND cs.""FieldName"" = 'LeadershipRelatedProblems'
						        )
						    ),
						    '{}'::jsonb
						) INTO response_data;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_village_issues_by_id: %', SQLERRM;
					END;				
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_create_village_issues(jsonb);
				DROP FUNCTION IF EXISTS public.fn_update_village_issues(jsonb);
				DROP FUNCTION IF EXISTS public.fn_village_issues_by_id(uuid);
            ");
        }
    }
}
