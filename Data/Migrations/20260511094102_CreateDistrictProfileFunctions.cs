using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDistrictProfileFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_create_district_profile(
					input_data jsonb
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_district_profile_id UUID;
						v_array_element TEXT;
						v_entity_name CONSTANT VARCHAR(50) := 'DistrictProfile';
						v_image_id TEXT;
					BEGIN
						-- Insert DistrictProfiles data
						BEGIN
							INSERT INTO public.""DistrictProfiles""(
								""Id"",
								""DistrictId"", 
								""ParkId"", 
								""AreaSize"", 
								""Population"", 
								""PopulationGrowthRate"", 
								""AreaOccupiedByPark"", 
								""RelationshipStatus"", 
								""AverageAnnualRainfall"", 
								""Landform"", 
								""RainfallPattern"", 
								""VegetationCharacteristics"", 
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'DistrictId')::UUID,
								(input_data->>'ParkId')::UUID,
								(input_data->>'AreaSize')::NUMERIC,
								(input_data->>'Population')::NUMERIC,
								(input_data->>'PopulationGrowthRate')::NUMERIC,
								(input_data->>'AreaOccupiedByPark')::NUMERIC,
								input_data->>'RelationshipStatus',
								(input_data->>'AverageAnnualRainfall')::NUMERIC,
								input_data->>'Landform',
								input_data->>'RainfallPattern',
								input_data->>'VegetationCharacteristics',
								(input_data->>'CreatedBy')::UUID,
								NOW()
							) RETURNING ""Id"" INTO v_district_profile_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'DistrictProfiles table error: %', SQLERRM;
						END;

						-- Insert Files Data
						BEGIN
							v_image_id := input_data->>'ImageId';
							IF v_image_id IS NOT NULL AND v_image_id != '' THEN
								UPDATE public.""Files""
								SET
									""EntityId"" = v_district_profile_id,
									""EntityName"" = v_entity_name,
									""UpdatedBy"" = (input_data->>'CreatedBy')::UUID,
									""UpdatedAt"" = NOW()
								WHERE ""Id"" = v_image_id::UUID;
							END IF;
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'Files table error: %', SQLERRM;
						END;
						
						-- Insert Tribes Data
						IF input_data->'Tribes' IS NOT NULL AND jsonb_array_length(input_data->'Tribes') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'Tribes')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
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
										'Tribes',
										v_district_profile_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (Tribes) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert LandHelds Data
						IF input_data->'LandHelds' IS NOT NULL AND jsonb_array_length(input_data->'LandHelds') > 0 THEN
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'LandHelds')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
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
										'LandHelds',
										v_district_profile_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (LandHelds) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return the created v_district_profile_id
						RETURN v_district_profile_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_district_profile: %', SQLERRM;
					END;			
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_district_profile_by_id(
					district_profile_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', dp.""Id"",
								'DistrictId', dp.""DistrictId"",
								'ParkId', dp.""ParkId"",
								'AreaSize', dp.""AreaSize"",
								'Population', dp.""Population"",
								'PopulationGrowthRate', dp.""PopulationGrowthRate"",
								'AreaOccupiedByPark', dp.""AreaOccupiedByPark"",
								'RelationshipStatus', dp.""RelationshipStatus"",
								'AverageAnnualRainfall', dp.""AverageAnnualRainfall"",
								'Landform', dp.""Landform"",
								'RainfallPattern', dp.""RainfallPattern"",
								'VegetationCharacteristics', dp.""VegetationCharacteristics"",
								'CreatedBy', dp.""CreatedBy"",
								'CreatedAt', dp.""CreatedAt"",
								'UpdatedBy', dp.""UpdatedBy"",
								'UpdatedAt', dp.""UpdatedAt"",
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'District', jsonb_build_object(
									'Id', d.""Id"",
									'Name', d.""Name"",
									'RegionId', d.""RegionId"",
									'CreatedAt', d.""CreatedAt"",
									'CreatedBy', d.""CreatedBy""
								),
								'Tribes', (
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
						            WHERE cs.""EntityId"" = dp.""Id""
						            AND cs.""FieldName"" = 'Tribes'
						        ),
								'LandHelds', (
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
						            WHERE cs.""EntityId"" = dp.""Id""
						            AND cs.""FieldName"" = 'LandHelds'
						        ),
								'Creator', jsonb_build_object(
									'Id', u.""Id"",
									'Username', u.""Username"",
									'Email', u.""Email""
								),
								'Updater', (
									SELECT jsonb_build_object(
										'Id', u.""Id"",
										'Username', u.""Username"",
										'Email', u.""Email""
									)
									FROM ""Users"" u
									WHERE u.""Id"" = dp.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""DistrictProfiles"" dp
						JOIN ""Parks"" p ON p.""Id"" = dp.""ParkId""
						JOIN ""Districts"" d ON d.""Id"" = dp.""DistrictId""
						JOIN ""Users"" u ON u.""Id"" = dp.""CreatedBy""
						WHERE dp.""Id"" = district_profile_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_district_profile_by_id: %', SQLERRM;
					END;	
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_update_district_profile(
					input_data jsonb,
					district_profile_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_district_profile_id UUID;
						v_array_element TEXT;
						v_entity_name CONSTANT VARCHAR(50) := 'DistrictProfile';
					BEGIN
						-- Convert district_profile_id parameter to UUID
						v_district_profile_id := district_profile_id::UUID;
						
						-- Update DistrictProfiles data
						BEGIN
							UPDATE public.""DistrictProfiles""
							SET
								""DistrictId"" = (input_data->>'DistrictId')::UUID,
								""ParkId"" = (input_data->>'ParkId')::UUID,
								""AreaSize"" = (input_data->>'AreaSize')::NUMERIC,
								""Population"" = (input_data->>'Population')::NUMERIC,
								""PopulationGrowthRate"" = (input_data->>'PopulationGrowthRate')::NUMERIC,
								""AreaOccupiedByPark"" = (input_data->>'AreaOccupiedByPark')::NUMERIC,
								""AverageAnnualRainfall"" = (input_data->>'AverageAnnualRainfall')::NUMERIC,
								""RelationshipStatus"" = input_data->>'RelationshipStatus',
								""Landform"" = input_data->>'Landform',
								""RainfallPattern"" = input_data->>'RainfallPattern',
								""VegetationCharacteristics"" = input_data->>'VegetationCharacteristics',
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_district_profile_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'DistrictProfiles table error: %', SQLERRM;
						END;
						
						-- Insert Tribes Data
						IF input_data->'Tribes' IS NOT NULL AND jsonb_array_length(input_data->'Tribes') > 0 THEN
						
							-- Delete existing Tribes
							DELETE FROM public.""CommunitySelections""
							WHERE ""EntityId"" = v_district_profile_id
							AND ""EntityName"" = v_entity_name
							AND ""FieldName"" = 'Tribes';
											
							FOR v_array_element IN 
								SELECT jsonb_array_elements_text(input_data->'Tribes')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
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
										'Tribes',
										v_district_profile_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (Tribes) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;

						-- Insert LandHelds Data
						IF input_data->'LandHelds' IS NOT NULL AND jsonb_array_length(input_data->'LandHelds') > 0 THEN
						
							-- Delete existing LandHelds
							DELETE FROM public.""CommunitySelections""
							WHERE ""EntityId"" = v_district_profile_id
							AND ""EntityName"" = v_entity_name
							AND ""FieldName"" = 'LandHelds';
											
							FOR v_array_element IN
								SELECT jsonb_array_elements_text(input_data->'LandHelds')
							LOOP
								BEGIN
									INSERT INTO public.""CommunitySelections""(
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
										'LandHelds',
										v_district_profile_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'CommunitySelections (LandHelds) table error: %', SQLERRM;
								END;    
							END LOOP;
						END IF;
						
						-- Return the created v_district_profile_id
						RETURN v_district_profile_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_update_district_profile: %', SQLERRM;
					END;							
				$BODY$;
            ");
            
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_district_profile_details(
					district_profile_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', dp.""Id"",
								'DistrictId', dp.""DistrictId"",
								'ParkId', dp.""ParkId"",
								'AreaSize', dp.""AreaSize"",
								'Population', dp.""Population"",
								'PopulationGrowthRate', dp.""PopulationGrowthRate"",
								'AreaOccupiedByPark', dp.""AreaOccupiedByPark"",
								'RelationshipStatus', dp.""RelationshipStatus"",
								'AverageAnnualRainfall', dp.""AverageAnnualRainfall"",
								'Landform', dp.""Landform"",
								'RainfallPattern', dp.""RainfallPattern"",
								'VegetationCharacteristics', dp.""VegetationCharacteristics"",
								'CreatedBy', dp.""CreatedBy"",
								'CreatedAt', dp.""CreatedAt"",
								'UpdatedBy', dp.""UpdatedBy"",
								'UpdatedAt', dp.""UpdatedAt"",
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'District', jsonb_build_object(
									'Id', d.""Id"",
									'Name', d.""Name"",
									'RegionId', d.""RegionId"",
									'CreatedAt', d.""CreatedAt"",
									'CreatedBy', d.""CreatedBy""
								),
								'Tribes', (
						            SELECT COALESCE(jsonb_agg(
						                jsonb_build_object(
						                    'Id', cs.""Id"",
						                    'Value', cs.""Value"",
						                    'OtherName', cs.""OtherName"",
											'EntityId', cs.""EntityId"",
						                    'EntityName', cs.""EntityName"",
						                    'CreatedAt', cs.""CreatedAt"",
						                    'CreatedBy', cs.""CreatedBy"",
											'Tribe', (
												SELECT jsonb_build_object(
													'Id', t.""Id"",
													'Name', t.""Name"",
													'CreatedAt', t.""CreatedAt"",
													'CreatedBy', t.""CreatedBy""
												)
												FROM ""Tribes"" t
												WHERE t.""Id"" = (cs.""Value"")::UUID
											)
						                )
						            ), '[]'::jsonb)
						            FROM ""CommunitySelections"" cs
						            WHERE cs.""EntityId"" = dp.""Id""
						            AND cs.""FieldName"" = 'Tribes'
						        ),
								'LandHelds', (
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
						            WHERE cs.""EntityId"" = dp.""Id""
						            AND cs.""FieldName"" = 'LandHelds'
						        ),
								'DistrictOfficials', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', gd.""Id"",
											'FullName', gd.""FullName"",
											'Position', gd.""Position"",
											'Mobile', gd.""Mobile"",
											'TelephoneNumber', gd.""TelephoneNumber"",
											'Address', gd.""Address"",
											'FieldName', gd.""FieldName"",
											'EntityId', gd.""EntityId"",
											'EntityName', gd.""EntityName"",
											'CreatedAt', gd.""CreatedAt"",
											'CreatedBy', gd.""CreatedBy"",
											'UpdatedAt', gd.""UpdatedAt"",
											'UpdatedBy', gd.""UpdatedBy""
										)
										ORDER BY gd.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""GovernmentLeaders"" gd
									WHERE gd.""EntityId"" = dp.""Id""
									AND gd.""FieldName"" = 'district_officials'
									AND gd.""DeletedAt"" IS NULL
								),
								'ParliamentMembers', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', gd.""Id"",
											'FullName', gd.""FullName"",
											'Position', gd.""Position"",
											'Mobile', gd.""Mobile"",
											'TelephoneNumber', gd.""TelephoneNumber"",
											'Address', gd.""Address"",
											'FieldName', gd.""FieldName"",
											'EntityId', gd.""EntityId"",
											'EntityName', gd.""EntityName"",
											'CreatedAt', gd.""CreatedAt"",
											'CreatedBy', gd.""CreatedBy"",
											'UpdatedAt', gd.""UpdatedAt"",
											'UpdatedBy', gd.""UpdatedBy""
										)
										ORDER BY gd.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""GovernmentLeaders"" gd
									WHERE gd.""EntityId"" = dp.""Id""
									AND gd.""FieldName"" = 'parliament_members'
									AND gd.""DeletedAt"" IS NULL
								),
								'GovernmentProgrammes', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'government_programmes'
									AND dc.""DeletedAt"" IS NULL
								),
								'BenefitsFromPark', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'benefit_from_park'
									AND dc.""DeletedAt"" IS NULL
								),
								'LanduseTrends', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'landuse_trend'
									AND dc.""DeletedAt"" IS NULL
								),
								'LanduseProblems', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'landuse_problem'
									AND dc.""DeletedAt"" IS NULL
								),
								'IssuesAffectingPark', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'issue_affecting_park'
									AND dc.""DeletedAt"" IS NULL
								),
								'NaturalResourceAreas', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'natural_resource_area'
									AND dc.""DeletedAt"" IS NULL
								),
								'MarketAreas', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'market_area'
									AND dc.""DeletedAt"" IS NULL
								),
								'BoundariesToPark', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'boundary_to_park'
									AND dc.""DeletedAt"" IS NULL
								),
								'TourismFacilities', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'tourism_facility'
									AND dc.""DeletedAt"" IS NULL
								),
								'EconomicActivities', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'economic_activities'
									AND dc.""DeletedAt"" IS NULL
								),
								'BorderingVillages', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dc.""Id"",
											'Name', dc.""Name"",
											'Description', dc.""Description"",
											'FieldName', dc.""FieldName"",
											'DistrictProfileId', dc.""DistrictProfileId"",
											'CreatedAt', dc.""CreatedAt"",
											'CreatedBy', dc.""CreatedBy"",
											'UpdatedAt', dc.""UpdatedAt"",
											'UpdatedBy', dc.""UpdatedBy""
										)
										ORDER BY dc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DistrictContexts"" dc
									WHERE dc.""DistrictProfileId"" = dp.""Id""
									AND dc.""FieldName"" = 'bordering_villages'
									AND dc.""DeletedAt"" IS NULL
								),
								'DevelopmentOrganizations', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', dg.""Id"",
											'Name', dg.""Name"",
											'TimeOfOperation', dg.""TimeOfOperation"",
											'Address', dg.""Address"",
											'TelephoneNumber', dg.""TelephoneNumber"",
											'ContactPersonName', dg.""ContactPersonName"",
											'ContactPersonMobile', dg.""ContactPersonMobile"",
											'AreaOfOperation', dg.""AreaOfOperation"",
											'TypeOfOperation', dg.""TypeOfOperation"",
											'Comment', dg.""Comment"",
											'DistrictProfileId', dg.""DistrictProfileId"",
											'CreatedAt', dg.""CreatedAt"",
											'CreatedBy', dg.""CreatedBy"",
											'UpdatedAt', dg.""UpdatedAt"",
											'UpdatedBy', dg.""UpdatedBy""
										)
										ORDER BY dg.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""DevelopmentOrganizations"" dg
									WHERE dg.""DistrictProfileId"" = dp.""Id""
									AND dg.""DeletedAt"" IS NULL
								),
								'Creator', jsonb_build_object(
									'Id', u.""Id"",
									'Username', u.""Username"",
									'Email', u.""Email""
								),
								'Updater', (
									SELECT jsonb_build_object(
										'Id', u.""Id"",
										'Username', u.""Username"",
										'Email', u.""Email""
									)
									FROM ""Users"" u
									WHERE u.""Id"" = dp.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""DistrictProfiles"" dp
						JOIN ""Parks"" p ON p.""Id"" = dp.""ParkId""
						JOIN ""Districts"" d ON d.""Id"" = dp.""DistrictId""
						JOIN ""Users"" u ON u.""Id"" = dp.""CreatedBy""
						WHERE dp.""Id"" = district_profile_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_district_profile_details: %', SQLERRM;
					END;	
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS public.fn_create_district_profile(jsonb);
				DROP FUNCTION IF EXISTS public.fn_district_profile_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_district_profile(jsonb, character varying);
				DROP FUNCTION IF EXISTS public.fn_district_profile_details(uuid);
				DROP FUNCTION IF EXISTS public.fn_district_profile_details(uuid);
            ");
        }
    }
}
