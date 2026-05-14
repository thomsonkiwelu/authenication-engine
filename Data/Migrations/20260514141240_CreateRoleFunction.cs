using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authentication_engine.Migrations
{
    /// <inheritdoc />
    public partial class CreateRoleFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION public.fn_roles(
	                page_number integer,
	                page_size integer,
	                search_text character varying DEFAULT ''::character varying,
	                system_application_id character varying DEFAULT ''::character varying
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

                            WITH role_data AS (
                                SELECT 
                                    r.""Id"",
                                    r.""Name"",
                                    r.""SystemApplicationId"",
                                    r.""Slug"",
                                    r.""Description"",
                                    r.""CreatedAt"",
                                    r.""CreatedBy"",
                                    sa.""Id"" AS SystemApplicationId,
                                    sa.""Name"" AS SystemApplicationName,	
                                    u.""Id"" AS UserId,
                                    u.""Username"" AS Username,
					                (
					                    SELECT sm.""Slug""
					                    FROM ""SystemModules"" sm
					                    WHERE sm.""SystemApplicationId"" = r.""SystemApplicationId""
					                    ORDER BY sm.""Name"" ASC
					                    LIMIT 1
					                ) AS ""SystemModuleSlugName"",
                                    ROW_NUMBER() OVER (ORDER BY r.""CreatedAt"" DESC) AS row_number,
                                    COUNT(*) OVER() AS full_count
                                FROM ""Roles"" r
                                JOIN ""SystemApplications"" sa ON sa.""Id"" = r.""SystemApplicationId""
                                JOIN ""Users"" u ON u.""Id"" = r.""CreatedBy""
                                WHERE
                                    (search_text IS NULL OR search_text = '' OR
                                    r.""Name"" ILIKE '%' || search_text || '%')
                                    AND r.""DeletedAt"" IS NULL
                                    AND (system_application_id IS NULL OR system_application_id = '' OR r.""SystemApplicationId"" = system_application_id::UUID)
                            ),
                            paginated_data AS (
                                SELECT * FROM role_data
                                WHERE row_number BETWEEN min_row_num AND max_row_num
                            )
                            SELECT 
                                jsonb_agg(
                                    jsonb_build_object(
                                        'RowNumber', row_number,
                                        'Id', ""Id"",
                                        'Name', ""Name"",
                                        'SystemApplicationId', ""SystemApplicationId"",
                                        'Slug', ""Slug"",
                                        'Description', ""Description"",
                                        'CreatedAt', ""CreatedAt"",
                                        'CreatedBy', ""CreatedBy"",
						                'SystemModuleSlugName', ""SystemModuleSlugName"",
                                        'SystemApplication', jsonb_build_object(
                                            'Id', SystemApplicationId,
                                            'Name', SystemApplicationName
                                        ),
                                        'Creator', jsonb_build_object(
                                            'Id', UserId,
                                            'Username', Username
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
                                RAISE EXCEPTION 'Error in fn_roles: %', SQLERRM;
                        END;
                $BODY$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP FUNCTION IF EXISTS public.fn_roles(integer, integer, character varying, character varying);
            ");
        }
    }
}
