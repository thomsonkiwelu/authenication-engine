using Microsoft.EntityFrameworkCore;

namespace authentication_engine.Shared
{
    public class ApplyFilters<T>
    {
        public static IQueryable<T> ApplySearch(
            IQueryable<T> query, 
            string search, 
            params string[] columns
        )
        {
            if (string.IsNullOrWhiteSpace(search) || columns.Length == 0)
                return query;

            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
            System.Linq.Expressions.Expression? orExpression = null;

            var searchLower = search.ToLower();

            foreach (var column in columns)
            {
                System.Linq.Expressions.Expression propertyAccess = parameter;
                foreach (var member in column.Split('.', StringSplitOptions.RemoveEmptyEntries))
                {
                    propertyAccess = System.Linq.Expressions.Expression.PropertyOrField(propertyAccess, member);
                }

                if (propertyAccess.Type != typeof(string))
                    continue;

                var nonNullString = System.Linq.Expressions.Expression.Coalesce(
                    propertyAccess,
                    System.Linq.Expressions.Expression.Constant(string.Empty)
                );

                var toLower = System.Linq.Expressions.Expression.Call(nonNullString, nameof(string.ToLower), null);
                var searchConst = System.Linq.Expressions.Expression.Constant(searchLower);
                var contains = System.Linq.Expressions.Expression.Call(toLower, nameof(string.Contains), null, searchConst);

                orExpression = orExpression == null ? contains : System.Linq.Expressions.Expression.OrElse(orExpression, contains);
            }

            if (orExpression == null)
                return query;

            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(orExpression, parameter);

            return query.Where(lambda);
        }

        
        public static IQueryable<T> ApplySorting(
            IQueryable<T> query,
            string? sortBy,
            bool sortDesc
        )
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query.OrderByDescending(e => EF.Property<object>(e!, "CreatedAt"));

            return (sortBy.ToLower(), sortDesc) switch
            {
                ("createdat", true) => query.OrderByDescending(e => EF.Property<object>(e!, "CreatedAt")),
                ("createdat", false) => query.OrderBy(e => EF.Property<object>(e!, "CreatedAt")),

                //INFO: Permission Table
                ("modeltype", true) => query.OrderByDescending(e => EF.Property<object>(e!, "ModelType")),
                ("modeltype", false) => query.OrderBy(e => EF.Property<object>(e!, "ModelType")),

                //INFO: Common for entity
                ("name", true) => query.OrderByDescending(e => EF.Property<object>(e!, "Name")),
                ("name", false) => query.OrderBy(e => EF.Property<object>(e!, "Name")),

                //INFO: Common for Ranks Table
                ("level", true) => query.OrderByDescending(e => EF.Property<object>(e!, "Level")),
                ("level", false) => query.OrderBy(e => EF.Property<object>(e!, "Level")),
                
                //INFO: Common for Species Table
                ("scientificname", true) => query.OrderByDescending(e => EF.Property<object>(e!, "ScientificName")),
                ("scientificname", false) => query.OrderBy(e => EF.Property<object>(e!, "ScientificName")),
                ("commonname", true) => query.OrderByDescending(e => EF.Property<object>(e!, "CommonName")),
                ("commonname", false) => query.OrderBy(e => EF.Property<object>(e!, "CommonName")),

                _ => query.OrderByDescending(e => EF.Property<object>(e!, "CreatedAt"))
            };
        }
    }
}
