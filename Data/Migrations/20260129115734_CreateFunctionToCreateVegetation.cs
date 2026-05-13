using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunctionToCreateVegetation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_create_vegetation(
					input_data jsonb)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						v_vegetation_id UUID;
						v_entity_name CONSTANT VARCHAR(50) := 'Vegetation';
						v_disturbance_element JSONB;
						v_flora_element JSONB;
						v_distance_sample_element JSONB;
						v_park_id UUID;
						v_image_id TEXT;
					BEGIN
						-- Get Park
						SELECT ""ParkId"" INTO v_park_id
						FROM ""Locations""
						WHERE ""Id"" = (input_data->>'LocalNameId')::UUID;
							
						-- Insert vegetation data
						BEGIN
							INSERT INTO public.""Vegetations""(
								""Id"", 
								""LocalNameId"",
								""ParkId"",
								""Session"",
								""Rainfall"", 
								""Temperature"", 
								""Altitude"", 
								""Slope"", 
								""SoilType"", 
								""VegetationZone"", 
								""VegetationType"",
								""Methodology"", 
								""Coordinates"",
								""PlotId"", 
								""PlotSize"",
								""VegetationCategory"",
								""StartCoordinate"", 
								""EndCoordinate"",
								""SpeciesId"", 
								""SpeciesCount"", 
								""CommonNumber"", 
								""OtherMethodology"",
								""Remark"", 
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								(input_data->>'LocalNameId')::UUID,
								v_park_id,
								input_data->>'Session',
								input_data->>'Rainfall',
								input_data->>'Temperature',
								input_data->>'Altitude',
								input_data->>'Slope',
								input_data->>'SoilType',
								input_data->>'VegetationZone',
								input_data->>'VegetationType',
								input_data->>'MethodologyType',
								NULLIF(COALESCE(input_data->>'Coordinates', ''),''),
								NULLIF(COALESCE(input_data->>'PlotId', ''),''),
								NULLIF(COALESCE(input_data->>'PlotSize', ''),''),
								NULLIF(COALESCE(input_data->>'VegetationCategory', ''),''),
								NULLIF(COALESCE(input_data->>'StartCoordinate', ''),''),
								NULLIF(COALESCE(input_data->>'EndCoordinate', ''),''),
								NULLIF(COALESCE(input_data->>'OtherSpeciesId', ''), '')::UUID,
								NULLIF(COALESCE(input_data->>'SpeciesCount', ''),''),
								NULLIF(COALESCE(input_data->>'CommonNumber', ''),''),
								NULLIF(COALESCE(input_data->>'OtherMethodology', ''),''),
								NULLIF(COALESCE(input_data->>'Remark', ''),''),
								(input_data->>'CreatedBy')::UUID,
								NOW()
							)RETURNING ""Id"" INTO v_vegetation_id;
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'Vegetations table error: %', SQLERRM;
						END;

						-- Insert lifeform data
						BEGIN
							INSERT INTO public.""LifeForms""(
								""Id"", 
								""Name"", 
								""FamilyName"", 
								""SpeciesId"", 
								""Height"",
								""Weight"", 
								""StemNumber"", 
								""Diameter"", 
								""Cover"", 
								""DiameterAtBreastHeight"", 
								""Circumference"", 
								""CanopyDiameter"", 
								""CanopyClosure"", 
								""VegetationId"",
								""CreatedBy"", 
								""CreatedAt""
							) VALUES (
								gen_random_uuid(),
								input_data->>'LifeFormType',
								input_data->>'FamilyName',
								(input_data->>'LifeFormSpeciesId')::UUID,
								NULLIF(COALESCE(input_data->>'Height', ''),''),
								NULLIF(COALESCE(input_data->>'Weight', ''),''),
								NULLIF(COALESCE(input_data->>'StemNumber', ''),''),
								NULLIF(COALESCE(input_data->>'Diameter', ''),''),
								NULLIF(COALESCE(input_data->>'Cover', ''),''),
								NULLIF(COALESCE(input_data->>'DiameterAtBreastHeight', ''),''),
								NULLIF(COALESCE(input_data->>'Circumference', ''),''),
								NULLIF(COALESCE(input_data->>'CanopyDiameter', ''),''),
								NULLIF(COALESCE(input_data->>'CanopyClosure', ''),''),
								v_vegetation_id,
								(input_data->>'CreatedBy')::UUID,
								NOW()
							);
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'LifeForms table error: %', SQLERRM;
						END;

						-- Insert Files Data
						BEGIN
							v_image_id := input_data->>'ImageId';
							IF v_image_id IS NOT NULL AND v_image_id != '' THEN
								UPDATE public.""Files""
								SET
									""EntityId"" = v_vegetation_id,
									""EntityName"" = v_entity_name,
									""UpdatedBy"" = (input_data->>'CreatedBy')::UUID,
									""UpdatedAt"" = NOW()
								WHERE ""Id"" = v_image_id::UUID;
							END IF;
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'Files table error: %', SQLERRM;
						END;

						-- Insert Disturbances Data
						IF input_data->'Disturbances' IS NOT NULL AND jsonb_array_length(input_data->'Disturbances') > 0 THEN
							FOR v_disturbance_element IN SELECT * FROM jsonb_array_elements(input_data->'Disturbances')
							LOOP
								BEGIN
									INSERT INTO public.""Disturbances""(
										""Id"",
										""Name"",
										""Quantity"", 
										""EntityId"", 
										""EntityName"", 
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_disturbance_element->>'Name',
										NULLIF(COALESCE(v_disturbance_element->>'Quantity', ''),''),
										v_vegetation_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'Disturbances table error: %', SQLERRM;
								END;
							END LOOP;
						END IF;

						-- Insert Floras Data
						IF input_data->'Floras' IS NOT NULL AND jsonb_array_length(input_data->'Floras') > 0 THEN
							FOR v_flora_element IN SELECT * FROM jsonb_array_elements(input_data->'Floras')
							LOOP
								BEGIN
									INSERT INTO public.""SpeciesOccurrences""(
										""Id"",
										""SpeciesId"",
										""EntityId"", 
										""EntityName"", 
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_flora_element->>'ScientificNameId')::UUID,
										v_vegetation_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'SpeciesOccurrences table error: %', SQLERRM;
								END;	
							END LOOP;
						END IF;

						-- Insert DistanceSample Data
						IF input_data->'CoordinateAlongTransects' IS NOT NULL AND jsonb_array_length(input_data->'CoordinateAlongTransects') > 0 THEN
							FOR v_distance_sample_element IN SELECT * FROM jsonb_array_elements(input_data->'CoordinateAlongTransects')
							LOOP
								BEGIN
									INSERT INTO public.""DistanceSamples""(
										""Id"", 
										""LongCoordinate"", 
										""FamilyName"", 
										""SpeciesId"", 
										""LeftDistance"", 
										""RightDistance"", 
										""EntityId"", 
										""EntityName"", 
										""CreatedBy"", 
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_distance_sample_element->>'LongCoordinate',
										v_distance_sample_element->>'FamilyName',
										(v_distance_sample_element->>'SpeciesId')::UUID,
										v_distance_sample_element->>'LeftDistance',
										v_distance_sample_element->>'RightDistance',
										v_vegetation_id,
										v_entity_name,
										(input_data->>'CreatedBy')::UUID,
										NOW()
									);
								EXCEPTION
									WHEN OTHERS THEN
										RAISE EXCEPTION 'DistanceSamples table error: %', SQLERRM;
								END;
							END LOOP;
						END IF;
						
						-- Return the created vegetation_id
						RETURN v_vegetation_id::TEXT;
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_create_vegetation: %', SQLERRM;
					END;	
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_create_vegetation(jsonb);
            ");
        }
    }
}
