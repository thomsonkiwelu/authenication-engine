using conservation_backend.Config;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace conservation_backend.Data.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20260429090000_FixFnVegetationsSignature")]
    public partial class FixFnVegetationsSignature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP FUNCTION IF EXISTS public.fn_vegetations(integer, integer, character varying, character varying, character varying, character varying, uuid[]);
            """);

            migrationBuilder.Sql("""
                CREATE OR REPLACE FUNCTION public.fn_vegetations(
                    page_number integer,
                    page_size integer,
                    search_text character varying DEFAULT ''::character varying,
                    vegetation_type character varying DEFAULT ''::character varying,
                    methodology_type character varying DEFAULT ''::character varying,
                    life_form_type character varying DEFAULT ''::character varying,
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
                        offset_value := (page_number - 1) * page_size;
                        min_row_num := offset_value + 1;
                        max_row_num := offset_value + page_size;

                        WITH vegetation_data AS (
                            SELECT 
                                veg."Id" AS vegetation_id,
                                veg."LocalNameId",
                                veg."Session",
                                veg."VegetationType",
                                veg."Methodology",
                                veg."CreatedAt",
                                veg."CreatedBy",
                                loc."Id" AS location_id,
                                loc."Name" AS location_name,
                                p."Id" AS parkId,
                                p."Name" AS parkName,
                                lf."Id" AS lifeform_id,
                                lf."Name" AS lifeform_name,
                                lf."FamilyName" AS lifeform_family,
                                sp."Id" AS species_id,
                                sp."CommonName" AS species_common_name,
                                sp."ScientificName" AS species_scientific_name,
                                u."Id" AS user_id,
                                u."Username" AS username,
                                ROW_NUMBER() OVER (ORDER BY veg."CreatedAt" DESC) AS row_number,
                                COUNT(*) OVER() AS full_count
                            FROM "Vegetations" veg
                            JOIN "Locations" loc ON loc."Id" = veg."LocalNameId"
                            JOIN "Parks" p ON p."Id" = veg."ParkId"
                            JOIN "LifeForms" lf ON lf."VegetationId" = veg."Id"
                            JOIN "Species" sp ON sp."Id" = lf."SpeciesId"
                            JOIN "Users" u ON u."Id" = veg."CreatedBy"
                            WHERE
                                (search_text IS NULL OR search_text = '' OR
                                loc."Name" ILIKE '%' || search_text || '%' OR
                                sp."ScientificName" ILIKE '%' || search_text || '%' OR
                                sp."CommonName" ILIKE '%' || search_text || '%')
                                AND veg."DeletedAt" IS NULL
                                AND (park_id IS NULL OR park_id = '' OR veg."ParkId" = park_id::UUID)
                                AND (vegetation_type IS NULL OR vegetation_type = '' OR veg."VegetationType" = vegetation_type)
                                AND (methodology_type IS NULL OR methodology_type = '' OR veg."Methodology" = methodology_type)
                                AND (life_form_type IS NULL OR life_form_type = '' OR lf."Name" = life_form_type)
                                AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR veg."ParkId" = ANY(park_ids))
                        ),
                        paginated_data AS (
                            SELECT * FROM vegetation_data
                            WHERE row_number BETWEEN min_row_num AND max_row_num
                        )
                        SELECT 
                            jsonb_agg(
                                jsonb_build_object(
                                    'RowNumber', row_number,
                                    'Id', vegetation_id,
                                    'LocalNameId', "LocalNameId",
                                    'Session', "Session",
                                    'VegetationType', "VegetationType",
                                    'Methodology', "Methodology",
                                    'CreatedAt', "CreatedAt",
                                    'CreatedBy', "CreatedBy",
                                    'Location', jsonb_build_object(
                                        'Id', location_id,
                                        'Name', location_name
                                    ),
                                    'Park', jsonb_build_object(
                                        'Id', parkId,
                                        'Name', parkName
                                    ),
                                    'LifeForm', jsonb_build_object(
                                        'Id', lifeform_id,
                                        'Name', lifeform_name,
                                        'FamilyName', lifeform_family
                                    ),
                                    'Species', jsonb_build_object(
                                        'Id', species_id,
                                        'CommonName', species_common_name,
                                        'ScientificName', species_scientific_name
                                    ),
                                    'CreatedByUser', jsonb_build_object(
                                        'Id', user_id,
                                        'Username', username
                                    )
                                )
                            ),
                            MAX(full_count)
                        INTO response_data, total_count
                        FROM paginated_data;

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
                            RAISE EXCEPTION 'Error in fn_create_vegetation: %', SQLERRM;
                    END;
                $BODY$;
            """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP FUNCTION IF EXISTS public.fn_vegetations(integer, integer, character varying, character varying, character varying, character varying, character varying, uuid[]);
            """);

            migrationBuilder.Sql("""
                CREATE OR REPLACE FUNCTION public.fn_vegetations(
                    page_number integer,
                    page_size integer,
                    search_text character varying DEFAULT ''::character varying,
                    vegetation_type character varying DEFAULT ''::character varying,
                    methodology_type character varying DEFAULT ''::character varying,
                    life_form_type character varying DEFAULT ''::character varying,
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
                        offset_value := (page_number - 1) * page_size;
                        min_row_num := offset_value + 1;
                        max_row_num := offset_value + page_size;

                        WITH vegetation_data AS (
                            SELECT 
                                veg."Id" AS vegetation_id,
                                veg."LocalNameId",
                                veg."Session",
                                veg."VegetationType",
                                veg."Methodology",
                                veg."CreatedAt",
                                veg."CreatedBy",
                                loc."Id" AS location_id,
                                loc."Name" AS location_name,
                                p."Id" AS parkId,
                                p."Name" AS parkName,
                                lf."Id" AS lifeform_id,
                                lf."Name" AS lifeform_name,
                                lf."FamilyName" AS lifeform_family,
                                sp."Id" AS species_id,
                                sp."CommonName" AS species_common_name,
                                sp."ScientificName" AS species_scientific_name,
                                u."Id" AS user_id,
                                u."Username" AS username,
                                ROW_NUMBER() OVER (ORDER BY veg."CreatedAt" DESC) AS row_number,
                                COUNT(*) OVER() AS full_count
                            FROM "Vegetations" veg
                            JOIN "Locations" loc ON loc."Id" = veg."LocalNameId"
                            JOIN "Parks" p ON p."Id" = veg."ParkId"
                            JOIN "LifeForms" lf ON lf."VegetationId" = veg."Id"
                            JOIN "Species" sp ON sp."Id" = lf."SpeciesId"
                            JOIN "Users" u ON u."Id" = veg."CreatedBy"
                            WHERE
                                (search_text IS NULL OR search_text = '' OR
                                loc."Name" ILIKE '%' || search_text || '%' OR
                                sp."ScientificName" ILIKE '%' || search_text || '%' OR
                                sp."CommonName" ILIKE '%' || search_text || '%')
                                AND veg."DeletedAt" IS NULL
                                AND (vegetation_type IS NULL OR vegetation_type = '' OR veg."VegetationType" = vegetation_type)
                                AND (methodology_type IS NULL OR methodology_type = '' OR veg."Methodology" = methodology_type)
                                AND (life_form_type IS NULL OR life_form_type = '' OR lf."Name" = life_form_type)
                                AND (park_ids IS NULL OR array_length(park_ids, 1) IS NULL OR veg."ParkId" = ANY(park_ids))
                        ),
                        paginated_data AS (
                            SELECT * FROM vegetation_data
                            WHERE row_number BETWEEN min_row_num AND max_row_num
                        )
                        SELECT 
                            jsonb_agg(
                                jsonb_build_object(
                                    'RowNumber', row_number,
                                    'Id', vegetation_id,
                                    'LocalNameId', "LocalNameId",
                                    'Session', "Session",
                                    'VegetationType', "VegetationType",
                                    'Methodology', "Methodology",
                                    'CreatedAt', "CreatedAt",
                                    'CreatedBy', "CreatedBy",
                                    'Location', jsonb_build_object(
                                        'Id', location_id,
                                        'Name', location_name
                                    ),
                                    'Park', jsonb_build_object(
                                        'Id', parkId,
                                        'Name', parkName
                                    ),
                                    'LifeForm', jsonb_build_object(
                                        'Id', lifeform_id,
                                        'Name', lifeform_name,
                                        'FamilyName', lifeform_family
                                    ),
                                    'Species', jsonb_build_object(
                                        'Id', species_id,
                                        'CommonName', species_common_name,
                                        'ScientificName', species_scientific_name
                                    ),
                                    'CreatedByUser', jsonb_build_object(
                                        'Id', user_id,
                                        'Username', username
                                    )
                                )
                            ),
                            MAX(full_count)
                        INTO response_data, total_count
                        FROM paginated_data;

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
                            RAISE EXCEPTION 'Error in fn_create_vegetation: %', SQLERRM;
                    END;
                $BODY$;
            """);
        }
    }
}
