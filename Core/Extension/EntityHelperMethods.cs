using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Extension
{
    public static class EntityHelperMethods
    {
        public static IQueryable<TEntity> WhereAllPropertiesAreEqual<TEntity, TProperty>(this IQueryable<TEntity> query, TProperty value)
        {
            var param = Expression.Parameter(typeof(TEntity));

            var predicate = PredicateBuilder.True<TEntity>();

            foreach (var fieldName in GetEntityFieldsToCompareTo<TEntity, TProperty>())
            {
                var predicateToAdd = Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(param, fieldName),
                        Expression.Constant(value)), param);

                predicate = predicate.Or(predicateToAdd);
            }

            return query.Where(predicate);
        }

        private static IEnumerable<string> GetEntityFieldsToCompareTo<TEntity, TProperty>()
        {
            Type entityType = typeof(TEntity);
            Type propertyType = typeof(TProperty);

            var fields = entityType.GetFields()
                                .Where(f => f.FieldType == propertyType)
                                .Select(f => f.Name);

            var properties = entityType.GetProperties()
                                    .Where(p => p.PropertyType == propertyType)
                                    .Select(p => p.Name);

            return fields.Concat(properties);
        }
    }
}