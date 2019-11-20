using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace Microting.eForm.Infrastructure.Extensions
{
    public static class IQueryableExtensions  {
//        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();
//
//        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
//
//        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");
//
//        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");
//
//        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
//
//        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");
//
//        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
//        {
//            if (!(query is EntityQueryable<TEntity>) && !(query is InternalDbSet<TEntity>))
//            {
//                throw new ArgumentException("Invalid query");
//            }
//
//            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
//            var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
//            var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
//            var queryModel = parser.GetParsedQuery(query.Expression);
//            var database = DataBaseField.GetValue(queryCompiler);
//            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
//            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
//            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
//            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
//            var sql = modelVisitor.Queries.First().ToString();
//
//            return sql;
//        }

        public static string ToSql<TEntity>(this IQueryable<TEntity> query, DbContext dbCtx)
        {
            try
            {
                IQueryModelGenerator modelGenerator = dbCtx.GetService<IQueryModelGenerator>();
                QueryModel queryModel = modelGenerator.ParseQuery(query.Expression);
                DatabaseDependencies databaseDependencies = dbCtx.GetService<DatabaseDependencies>();
                QueryCompilationContext queryCompilationContext =
                    databaseDependencies.QueryCompilationContextFactory.Create(false);
                RelationalQueryModelVisitor modelVisitor =
                    (RelationalQueryModelVisitor) queryCompilationContext.CreateQueryModelVisitor();
                modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
                var sql = modelVisitor.Queries.First().ToString();
                return sql;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}