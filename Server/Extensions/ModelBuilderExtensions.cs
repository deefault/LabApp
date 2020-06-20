using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LabApp.Server.Data.Models.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using static LabApp.Server.Extensions.DbContextExtensions;

namespace LabApp.Server.Extensions
{
    public static class ModelBuilderExtensions
    {
        static void SetQueryFilter<TEntity, TEntityInterface>(
            this ModelBuilder builder,
            Expression<Func<TEntityInterface, bool>> filterExpression)
            where TEntityInterface : class
            where TEntity : class, TEntityInterface
        {
            Expression<Func<TEntity, bool>> concreteExpression = filterExpression
                .Convert<TEntityInterface, TEntity>();
            builder.Entity<TEntity>()
                .HasQueryFilter(concreteExpression);
        }

        static readonly MethodInfo SetQueryFilterMethod = typeof(ModelBuilderExtensions)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));

        static void SetEntityQueryFilter<TEntityInterface>(
            this ModelBuilder builder,
            Type entityType,
            Expression<Func<TEntityInterface, bool>> filterExpression)
        {
            SetQueryFilterMethod
                .MakeGenericMethod(entityType, typeof(TEntityInterface))
                .Invoke(null, new object[] {builder, filterExpression});
        }

        public static void SetQueryFilterOnAllEntities<TEntityInterface>(
            this ModelBuilder builder,
            Expression<Func<TEntityInterface, bool>> filterExpression)
        {
            foreach (Type type in builder.GetImplementations<TEntityInterface>())
            {
                builder.SetEntityQueryFilter<TEntityInterface>(
                    type,
                    filterExpression);
            }
        }

        public static void SetInsertedTrackableDefaults(this ModelBuilder builder, DatabaseFacade database)
        {
            foreach (Type type in GetImplementations<IInsertedTrackable>(builder))
            {
                builder.Entity(type, x =>
                    x.Property(nameof(IInsertedTrackable.Inserted))
                        .HasDefaultValueSql(GetDefaultValueForDateTime(database))
                );
            }
        }

        public static IEnumerable<Type> GetImplementations<TEntityInterface>(this ModelBuilder builder)
        {
            return builder.Model.GetEntityTypes()
                .Where(t => t.BaseType == null)
                .Select(t => t.ClrType)
                .Where(t => typeof(TEntityInterface).IsAssignableFrom(t));
        }
    }
}