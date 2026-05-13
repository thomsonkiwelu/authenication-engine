using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunctionToVegetationById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_vegetations_by_id(
					vegetation_id uuid)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE
				    response_data JSONB;
				BEGIN
				    SELECT COALESCE(
				        jsonb_build_object(
				            'Id', veg.""Id"", 
				            'LocalNameId', veg.""LocalNameId"", 
				            'Session', veg.""Session"",
				            'Rainfall', veg.""Rainfall"",
				            'Temperature', veg.""Temperature"",
				            'Altitude', veg.""Altitude"",
				            'Slope', veg.""Slope"",
				            'SoilType', veg.""SoilType"",
				            'VegetationType', veg.""VegetationType"",
				            'Methodology', veg.""Methodology"",
				            'VegetationZone', veg.""VegetationZone"",
				            'Coordinates', veg.""Coordinates"",
				            'SpeciesId', veg.""SpeciesId"",
				            'SpeciesCount', veg.""SpeciesCount"",
				            'CommonNumber', veg.""CommonNumber"",
				            'PlotId', veg.""PlotId"",
				            'VegetationCategory', veg.""VegetationCategory"",
				            'StartCoordinate', veg.""StartCoordinate"",
				            'EndCoordinate', veg.""EndCoordinate"",
				            'Remark', veg.""Remark"",
				            'OtherMethodology', veg.""OtherMethodology"",
				            'PlotSize', veg.""PlotSize"",
							'PlotSize', veg.""PlotSize"",
				            'CreatedBy', veg.""CreatedBy"",
				            'CreatedAt', veg.""CreatedAt"",
							'UpdatedAt', veg.""UpdatedAt"",
				            'UpdatedBy', veg.""UpdatedBy"",
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
								WHERE sp.""Id"" = veg.""SpeciesId""
							),
				            'LifeForm', jsonb_build_object(
				                'Id', lf.""Id"",
				                'Name', lf.""Name"",
				                'FamilyName', lf.""FamilyName"",
				                'SpeciesId', lf.""SpeciesId"",
				                'Height', lf.""Height"",
				                'Weight', lf.""Weight"",
				                'StemNumber', lf.""StemNumber"",
				                'Diameter', lf.""Diameter"",
				                'Cover', lf.""Cover"",
				                'DiameterAtBreastHeight', lf.""DiameterAtBreastHeight"",
				                'Circumference', lf.""Circumference"",
				                'CanopyDiameter', lf.""CanopyDiameter"",
				                'CanopyClosure', lf.""CanopyClosure"",
				                'VegetationId', lf.""VegetationId"",
				                'CreatedBy', lf.""CreatedBy"",
				                'CreatedAt', lf.""CreatedAt"",
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
				                    WHERE sp.""Id"" = lf.""SpeciesId""
				                )
				            ),
							'Disturbances', (
								SELECT COALESCE(jsonb_agg(
									jsonb_build_object(
										'Id', dis.""Id"",
										'Name', dis.""Name"",
										'Quantity', dis.""Quantity"",
										'EntityId', dis.""EntityId"",
										'EntityName', dis.""EntityName"",
										'CreatedAt', dis.""CreatedAt"",
										'CreatedBy', dis.""CreatedBy""
									)
								), '[]'::jsonb)
				                FROM ""Disturbances"" dis
				                WHERE dis.""EntityId"" = veg.""Id""
							),
							'DistanceSamples', (
								SELECT COALESCE(jsonb_agg(
									jsonb_build_object(
										'Id', disa.""Id"",
										'LongCoordinate', disa.""LongCoordinate"",
										'FamilyName', disa.""FamilyName"",
										'SpeciesId', disa.""SpeciesId"",
										'LeftDistance', disa.""LeftDistance"",
										'RightDistance', disa.""RightDistance"",
										'EntityId', disa.""EntityId"",
										'EntityName', disa.""EntityName"",
										'CreatedAt', disa.""CreatedAt"",
										'CreatedBy', disa.""CreatedBy"",
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
						                    WHERE sp.""Id"" = disa.""SpeciesId""
						                )
									)
								), '[]'::jsonb)
				                FROM ""DistanceSamples"" disa
				                WHERE disa.""EntityId"" = veg.""Id""
							),
							'SpeciesOccurrences', (
								SELECT COALESCE(jsonb_agg(
									jsonb_build_object(
										'Id', spo.""Id"",
										'SpeciesId', spo.""SpeciesId"",
										'EntityId', spo.""EntityId"",
										'EntityName', spo.""EntityName"",
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
				                WHERE spo.""EntityId"" = veg.""Id""
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
								WHERE u.""Id"" = veg.""UpdatedBy""
							)
				        ),
				        '{}'::jsonb
				    ) INTO response_data
				    FROM ""Vegetations"" veg
				    LEFT JOIN ""Locations"" loc ON loc.""Id"" = veg.""LocalNameId""
				    LEFT JOIN ""LifeForms"" lf ON lf.""VegetationId"" = veg.""Id""
				    LEFT JOIN ""Users"" u ON u.""Id"" = veg.""CreatedBy""
				    WHERE veg.""Id"" = vegetation_id;
				    
					-- data to be return
				    RETURN COALESCE(response_data, '{}'::jsonb);
				    
				EXCEPTION
				    WHEN OTHERS THEN
				        RAISE EXCEPTION 'Error in fn_vegetations_by_id: %', SQLERRM;
				END;
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_vegetations_by_id(uuid);
            ");
        }
    }
}
