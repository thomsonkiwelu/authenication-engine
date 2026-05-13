using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateVillageProfileFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.fn_village_profile_by_id(
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
								'Id', vp.""Id"",
								'VillageId', vp.""VillageId"",
								'ParkId', vp.""ParkId"",
								'VillageSize', vp.""VillageSize"",
								'NumberOfHousehold', vp.""NumberOfHousehold"",
								'NumberOfMale', vp.""NumberOfMale"",
								'NumberOfFemale', vp.""NumberOfFemale"",
								'NumberOfCow', vp.""NumberOfCow"",
								'NumberOfSheep', vp.""NumberOfSheep"",
								'NumberOfGoat', vp.""NumberOfGoat"",
								'NumberOfDog', vp.""NumberOfDog"",
								'Population', vp.""Population"",
								'LandStatus', vp.""LandStatus"",				
								'CreatedBy', vp.""CreatedBy"",
								'CreatedAt', vp.""CreatedAt"",
								'UpdatedBy', vp.""UpdatedBy"",
								'UpdatedAt', vp.""UpdatedAt"",
								'Village', jsonb_build_object(
									'Id', v.""Id"",
									'Name', v.""Name"",
									'RegionId', v.""RegionId"",
									'DistrictId', v.""DistrictId"",
									'DivisionId', v.""DivisionId"",
									'WardId', v.""WardId"",
									'CreatedAt', v.""CreatedAt"",
									'CreatedBy', v.""CreatedBy""
								),
								'Park', jsonb_build_object(
									'Id', p.""Id"",
									'Name', p.""Name"",
									'Code', p.""Code"",
									'Zone', p.""Zone"",
									'CreatedAt', p.""CreatedAt"",
									'CreatedBy', p.""CreatedBy""
								),
								'GovernmentLeaders', (
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
									WHERE gd.""EntityId"" = vp.""Id""
									AND gd.""DeletedAt"" IS NULL
								),
								'LandUsePractices', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', vc.""Id"",
											'Name', vc.""Name"",
											'Data', vc.""Data"",
											'Description', vc.""Description"",
											'FieldName', vc.""FieldName"",
											'EntityId', vc.""EntityId"",
											'EntityName', vc.""EntityName"",
											'CreatedAt', vc.""CreatedAt"",
											'CreatedBy', vc.""CreatedBy"",
											'UpdatedAt', vc.""UpdatedAt"",
											'UpdatedBy', vc.""UpdatedBy""
										)
										ORDER BY vc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""VillageContexts"" vc
									WHERE vc.""EntityId"" = vp.""Id""
									AND vc.""FieldName"" = 'land_use_practices'
									AND vc.""DeletedAt"" IS NULL
								),
								'VillageFacilities', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', vc.""Id"",
											'Name', vc.""Name"",
											'Data', vc.""Data"",
											'Description', vc.""Description"",
											'FieldName', vc.""FieldName"",
											'EntityId', vc.""EntityId"",
											'EntityName', vc.""EntityName"",
											'CreatedAt', vc.""CreatedAt"",
											'CreatedBy', vc.""CreatedBy"",
											'UpdatedAt', vc.""UpdatedAt"",
											'UpdatedBy', vc.""UpdatedBy""
										)
										ORDER BY vc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""VillageContexts"" vc
									WHERE vc.""EntityId"" = vp.""Id""
									AND vc.""FieldName"" = 'village_facility'
									AND vc.""DeletedAt"" IS NULL
								),
								'CommunityOrganizations', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', vc.""Id"",
											'Name', vc.""Name"",
											'Data', vc.""Data"",
											'Description', vc.""Description"",
											'FieldName', vc.""FieldName"",
											'EntityId', vc.""EntityId"",
											'EntityName', vc.""EntityName"",
											'CreatedAt', vc.""CreatedAt"",
											'CreatedBy', vc.""CreatedBy"",
											'UpdatedAt', vc.""UpdatedAt"",
											'UpdatedBy', vc.""UpdatedBy""
										)
										ORDER BY vc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""VillageContexts"" vc
									WHERE vc.""EntityId"" = vp.""Id""
									AND vc.""FieldName"" = 'community_based_organization'
									AND vc.""DeletedAt"" IS NULL
								),
								'CommunityProjects', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', vc.""Id"",
											'Name', vc.""Name"",
											'Data', vc.""Data"",
											'Description', vc.""Description"",
											'FieldName', vc.""FieldName"",
											'EntityId', vc.""EntityId"",
											'EntityName', vc.""EntityName"",
											'CreatedAt', vc.""CreatedAt"",
											'CreatedBy', vc.""CreatedBy"",
											'UpdatedAt', vc.""UpdatedAt"",
											'UpdatedBy', vc.""UpdatedBy""
										)
										ORDER BY vc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""VillageContexts"" vc
									WHERE vc.""EntityId"" = vp.""Id""
									AND vc.""FieldName"" = 'community_projects'
									AND vc.""DeletedAt"" IS NULL
								),
								'NonGovernmentOrganizations', (
									SELECT COALESCE(jsonb_agg(
										jsonb_build_object(
											'Id', vc.""Id"",
											'Name', vc.""Name"",
											'Data', vc.""Data"",
											'Description', vc.""Description"",
											'FieldName', vc.""FieldName"",
											'EntityId', vc.""EntityId"",
											'EntityName', vc.""EntityName"",
											'CreatedAt', vc.""CreatedAt"",
											'CreatedBy', vc.""CreatedBy"",
											'UpdatedAt', vc.""UpdatedAt"",
											'UpdatedBy', vc.""UpdatedBy"",
											'Identifications', (
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
									            WHERE cs.""EntityId"" = vc.""Id""
									            AND cs.""FieldName"" = 'NonGovernmentOrganization'
											)
										)
										ORDER BY vc.""CreatedAt"" DESC
									), '[]'::jsonb)
									FROM ""VillageContexts"" vc
									WHERE vc.""EntityId"" = vp.""Id""
									AND vc.""FieldName"" = 'non_government_organization'
									AND vc.""DeletedAt"" IS NULL
								),
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
									WHERE u.""Id"" = vp.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""VillageProfiles"" vp
						JOIN ""Users"" u ON u.""Id"" = vp.""CreatedBy""
						JOIN ""Parks"" p ON p.""Id"" = vp.""ParkId""
						JOIN ""Villages"" v ON v.""Id"" = vp.""VillageId""
						WHERE vp.""Id"" = village_profile_id;
						
						-- Data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_village_profile_by_id: %', SQLERRM;
					END;			
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS public.fn_village_profile_by_id(uuid);
            ");
        }
    }
}
