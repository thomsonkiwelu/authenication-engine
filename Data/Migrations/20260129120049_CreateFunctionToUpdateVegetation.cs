using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunctionToUpdateVegetation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_vegetation(
					input_data jsonb,
					vegetation_id character varying
				)
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
						-- Update vegetation data
						BEGIN
							-- Convert vegetation_id parameter to UUID
							v_vegetation_id := vegetation_id::UUID;

							-- Get ParkId
							SELECT ""ParkId"" INTO v_park_id 
							FROM ""Locations"" 
							WHERE ""Id"" = (input_data->>'LocalNameId')::UUID;
						
							UPDATE public.""Vegetations""
							SET
								""LocalNameId"" = (input_data->>'LocalNameId')::UUID,
								""ParkId"" = v_park_id,
								""Session"" = input_data->>'Session',
								""Rainfall"" = input_data->>'Rainfall',
								""Temperature"" = input_data->>'Temperature',
								""Altitude"" = input_data->>'Altitude',
								""Slope"" = input_data->>'Slope',
								""SoilType"" = input_data->>'SoilType',
								""VegetationZone"" = input_data->>'VegetationZone',
								""VegetationType"" = input_data->>'VegetationType',
								""Methodology"" = input_data->>'MethodologyType',
								""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''), ''),
								""PlotId"" = NULLIF(COALESCE(input_data->>'PlotId', ''), ''),
								""PlotSize"" = NULLIF(COALESCE(input_data->>'PlotSize', ''), ''),
								""VegetationCategory"" = NULLIF(COALESCE(input_data->>'VegetationCategory', ''), ''),
								""StartCoordinate"" = NULLIF(COALESCE(input_data->>'StartCoordinate', ''), ''),
								""EndCoordinate"" = NULLIF(COALESCE(input_data->>'EndCoordinate', ''), ''),
								""SpeciesId"" = NULLIF(COALESCE(input_data->>'OtherSpeciesId', ''), '')::UUID,
								""SpeciesCount"" = NULLIF(COALESCE(input_data->>'SpeciesCount', ''), ''),
								""CommonNumber"" = NULLIF(COALESCE(input_data->>'CommonNumber', ''), ''),
								""OtherMethodology"" = NULLIF(COALESCE(input_data->>'OtherMethodology', ''), ''),
								""Remark"" = NULLIF(COALESCE(input_data->>'Remark', ''), ''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""Id"" = v_vegetation_id
							RETURNING ""Id"" INTO v_vegetation_id;
							-- EXCEPTION HANDLING
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'Vegetations table error: %', SQLERRM;
						END;

						-- Insert Files Data
						BEGIN
							v_image_id := input_data->>'ImageId';
							IF v_image_id IS NOT NULL AND v_image_id != '' THEN
								UPDATE public.""Files""
								SET
									""EntityId"" = v_vegetation_id,
									""EntityName"" = v_entity_name,
									""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
									""UpdatedAt"" = NOW()
								WHERE ""Id"" = v_image_id::UUID;
							END IF;
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'Files table error: %', SQLERRM;
						END;

						-- Update lifeform data
						BEGIN
							UPDATE public.""LifeForms""
							SET
								""Name"" = input_data->>'LifeFormType',
								""FamilyName"" = input_data->>'FamilyName',
								""SpeciesId"" = (input_data->>'LifeFormSpeciesId')::UUID,
								""Height"" = NULLIF(COALESCE(input_data->>'Height', ''), ''),
								""Weight"" = NULLIF(COALESCE(input_data->>'Weight', ''), ''),
								""StemNumber"" = NULLIF(COALESCE(input_data->>'StemNumber', ''), ''),
								""Diameter"" = NULLIF(COALESCE(input_data->>'Diameter', ''), ''),
								""Cover"" = NULLIF(COALESCE(input_data->>'Cover', ''), ''),
								""DiameterAtBreastHeight"" = NULLIF(COALESCE(input_data->>'DiameterAtBreastHeight', ''), ''), 
								""Circumference"" = NULLIF(COALESCE(input_data->>'Circumference', ''), ''),
								""CanopyDiameter"" = NULLIF(COALESCE(input_data->>'CanopyDiameter', ''), ''),
								""CanopyClosure"" = NULLIF(COALESCE(input_data->>'CanopyClosure', ''), ''),
								""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
								""UpdatedAt"" = NOW()
							WHERE ""VegetationId"" = v_vegetation_id;
							-- EXCEPTION HANDLING
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'LifeForms table error: %', SQLERRM;
						END;

						-- Update Disturbances Data
						BEGIN
							IF input_data->'Disturbances' IS NOT NULL AND jsonb_array_length(input_data->'Disturbances') > 0 THEN
								-- Delete existing disturbances
								DELETE FROM public.""Disturbances""
								WHERE ""EntityId"" = v_vegetation_id AND ""EntityName"" = v_entity_name;
								-- Insert disturbances
								FOR v_disturbance_element IN SELECT * FROM jsonb_array_elements(input_data->'Disturbances')
								LOOP
									INSERT INTO public.""Disturbances""(
										""Id"",
										""Name"",
										""Quantity"",
										""EntityId"", 
										""EntityName"", 
										""UpdatedBy"",
										""UpdatedAt"",
										""CreatedBy"",
										""CreatedAt""
									) VALUES (
										gen_random_uuid(),
										v_disturbance_element->>'Name',
										NULLIF(COALESCE(v_disturbance_element->>'Quantity', ''), ''),
										v_vegetation_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								END LOOP;
							ELSE
								-- Only delete if there are existing disturbances
								IF EXISTS (
									SELECT 1 FROM public.""Disturbances"" 
									WHERE ""EntityId"" = v_vegetation_id AND ""EntityName"" = v_entity_name
								) THEN
									DELETE FROM public.""Disturbances""
									WHERE ""EntityId"" = v_vegetation_id AND ""EntityName"" = v_entity_name;
								END IF;
							END IF;	
							-- EXCEPTION HANDLING
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'Disturbances table error: %', SQLERRM;
						END;		

						-- Update Floras Data
						BEGIN
							IF input_data->'Floras' IS NOT NULL AND jsonb_array_length(input_data->'Floras') > 0 THEN
								-- Delete existing SpeciesOccurrences
									DELETE FROM public.""SpeciesOccurrences""
									WHERE ""EntityId"" = v_vegetation_id AND ""EntityName"" = v_entity_name;
								-- Insert Flora data	
								FOR v_flora_element IN SELECT * FROM jsonb_array_elements(input_data->'Floras')
								LOOP
									INSERT INTO public.""SpeciesOccurrences""(
										""Id"",
										""SpeciesId"",
										""EntityId"", 
										""EntityName"", 
										""CreatedBy"",
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										(v_flora_element->>'ScientificNameId')::UUID,
										v_vegetation_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								END LOOP;
							ELSE
								-- Only delete if there are existing SpeciesOccurrences
								IF EXISTS (
									SELECT 1 FROM public.""SpeciesOccurrences"" 
									WHERE ""EntityId"" = v_vegetation_id AND ""EntityName"" = v_entity_name
								) THEN
									DELETE FROM public.""SpeciesOccurrences"" 
									WHERE ""EntityId"" = v_vegetation_id AND ""EntityName"" = v_entity_name;
								END IF;
							END IF;
							-- EXCEPTION HANDLING
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'SpeciesOccurrences table error: %', SQLERRM;
						END;				

						-- Update DistanceSample Data
						BEGIN
							IF input_data->'CoordinateAlongTransects' IS NOT NULL AND jsonb_array_length(input_data->'CoordinateAlongTransects') > 0 THEN
								-- Delete existing DistanceSamples
									DELETE FROM public.""DistanceSamples""
									WHERE ""EntityId"" = v_vegetation_id AND ""EntityName"" = v_entity_name;
								-- Insert DistanceSample Data
								FOR v_distance_sample_element IN SELECT * FROM jsonb_array_elements(input_data->'CoordinateAlongTransects')
								LOOP
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
										""CreatedAt"",
										""UpdatedBy"",
										""UpdatedAt""
									) VALUES (
										gen_random_uuid(),
										v_distance_sample_element->>'LongCoordinate',
										v_distance_sample_element->>'FamilyName',
										(v_distance_sample_element->>'SpeciesId')::UUID,
										v_distance_sample_element->>'LeftDistance',
										v_distance_sample_element->>'RightDistance',
										v_vegetation_id,
										v_entity_name,
										(input_data->>'UpdatedBy')::UUID,
										NOW(),
										(input_data->>'UpdatedBy')::UUID,
										NOW()
									);
								END LOOP;
							ELSE
								-- Only delete if there are existing DistanceSamples
								IF EXISTS (
									SELECT 1 FROM public.""DistanceSamples"" 
									WHERE ""EntityId"" = v_vegetation_id AND ""EntityName"" = v_entity_name
								) THEN
									DELETE FROM public.""DistanceSamples"" 
									WHERE ""EntityId"" = v_vegetation_id AND ""EntityName"" = v_entity_name;
								END IF;
							END IF;
							-- EXCEPTION HANDLING	
							EXCEPTION
								WHEN OTHERS THEN
									RAISE EXCEPTION 'DistanceSamples table error: %', SQLERRM;
						END;
						
						-- Return the created vegetation_id
						RETURN v_vegetation_id::TEXT;
						
						-- GLOBAL EXCEPTION HANDLING
						EXCEPTION
							WHEN OTHERS THEN
								RAISE EXCEPTION 'Error in fn_update_vegetation: %', SQLERRM;
					END;				
				$BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_update_vegetation(jsonb, character varying);
            ");
        }
    }
}
