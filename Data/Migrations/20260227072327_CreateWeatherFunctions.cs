using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateWeatherFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_weather_by_id(
					weather_id uuid
				)
				    RETURNS jsonb
				    LANGUAGE 'plpgsql'
				AS $BODY$
					DECLARE
						response_data JSONB;
					BEGIN
						SELECT COALESCE(
							jsonb_build_object(
								'Id', we.""Id"",
								'StationId', we.""StationId"",
								'TmaRegistrationNumber', we.""TmaRegistrationNumber"",
								'CollectionMethod', we.""CollectionMethod"",
								'RainfallFrequency', we.""RainfallFrequency"",
								'Rainfall', we.""Rainfall"",
								'MinimumTemperature', we.""MinimumTemperature"",
								'MaximumTemperature', we.""MaximumTemperature"",
								'MeanTemperature', we.""MeanTemperature"",
								'WindDirection', we.""WindDirection"",
								'WindSpeed', we.""WindSpeed"",
								'AverageWindSpeed', we.""AverageWindSpeed"",
								'DryHumidity', we.""DryHumidity"",
								'WetHumidity', we.""WetHumidity"",
								'AverageHumidity', we.""AverageHumidity"",
								'CloudCover', we.""CloudCover"",
								'Sunshine', we.""Sunshine"",
								'Pressure', we.""Pressure"",
								'Evaporation', we.""Evaporation"",
								'Radiation', we.""Radiation"",
								'DeviceName', we.""DeviceName"",
								'Coordinates', we.""Coordinates"",
								'CreatedBy', we.""CreatedBy"",
								'CreatedAt', we.""CreatedAt"",
								'UpdatedBy', we.""UpdatedBy"",
								'UpdatedAt', we.""UpdatedAt"",
								'Station', jsonb_build_object(
									'Id', sta.""Id"",
									'Name', sta.""Name"",
									'Type', sta.""Type"",
									'ParkId', sta.""ParkId"",
									'CreatedBy', sta.""CreatedBy"",
									'CreatedAt', sta.""CreatedAt"",
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
										WHERE p.""Id"" = sta.""ParkId""
									)
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
									WHERE u.""Id"" = we.""UpdatedBy""
								)
							),
							'{}'::jsonb
						) INTO response_data
						FROM ""Weather"" we
						JOIN ""Stations"" sta ON sta.""Id"" = we.""StationId""
						JOIN ""Users"" u ON u.""Id"" = we.""CreatedBy""
						WHERE we.""Id"" = weather_id;
						
						-- data to be return
						RETURN COALESCE(response_data, '{}'::jsonb);
						
					EXCEPTION
						WHEN OTHERS THEN
							RAISE EXCEPTION 'Error in fn_weather_by_id: %', SQLERRM;
					END;		
				$BODY$;
			");
            
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_update_weather(
					input_data jsonb,
					weather_id character varying
				)
				    RETURNS text
				    LANGUAGE 'plpgsql'
				AS $BODY$
				DECLARE
				    v_weather_id UUID;
					v_park_id UUID;
				BEGIN 
				    -- Update weather data
				    BEGIN
				        -- Convert weather_id parameter to UUID
				        v_weather_id := weather_id::UUID;

						-- Get ParkId
						SELECT ""ParkId"" INTO v_park_id 
						FROM ""Stations""
						WHERE ""Id"" = (input_data->>'StationId')::UUID;
				        
				        UPDATE public.""Weather""
				        SET
				            ""StationId"" = (input_data->>'StationId')::UUID,
							""ParkId"" = v_park_id,
							""TmaRegistrationNumber"" = input_data->>'TmaRegistrationNumber',
							""CollectionMethod"" = input_data->>'CollectionMethod',
							""RainfallFrequency"" = input_data->>'RainfallFrequency',
							""Rainfall"" = input_data->>'Rainfall',
							""MinimumTemperature"" = input_data->>'MinimumTemperature',
							""MaximumTemperature"" = input_data->>'MaximumTemperature',
							""MeanTemperature"" = input_data->>'MeanTemperature',
							""WindDirection"" = input_data->>'WindDirection',
							""WindSpeed"" = input_data->>'WindSpeed',
							""AverageWindSpeed"" = input_data->>'AverageWindSpeed',
							""DryHumidity"" = input_data->>'DryHumidity',
							""WetHumidity"" = input_data->>'WetHumidity',
							""AverageHumidity"" = input_data->>'AverageHumidity',
							""CloudCover"" = input_data->>'CloudCover',
							""Sunshine"" = input_data->>'Sunshine',
							""Pressure"" = input_data->>'Pressure',
							""Evaporation"" = input_data->>'Evaporation',
							""Radiation"" = input_data->>'Radiation',
							""DeviceName"" = input_data->>'DeviceName',
							""Coordinates"" = NULLIF(COALESCE(input_data->>'Coordinates', ''), ''),
				            ""UpdatedBy"" = (input_data->>'UpdatedBy')::UUID,
				            ""UpdatedAt"" = NOW()
				        WHERE ""Id"" = v_weather_id
				        RETURNING ""Id"" INTO v_weather_id;
						
					    EXCEPTION
					        WHEN OTHERS THEN
					            RAISE EXCEPTION 'Weather table error: %', SQLERRM;
				    END;

				    -- Return the updated weather_id
				    RETURN v_weather_id::TEXT;
				    
					EXCEPTION
					    WHEN OTHERS THEN
					        RAISE EXCEPTION 'Error in fn_update_weather: %', SQLERRM;
				END;        
				$BODY$;
			");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP FUNCTION IF EXISTS public.fn_weather_by_id(uuid);
				DROP FUNCTION IF EXISTS public.fn_update_weather(jsonb, character varying);
            ");
        }
    }
}
