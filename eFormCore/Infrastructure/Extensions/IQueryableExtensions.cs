/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Microting.eForm.Infrastructure.Extensions
{
    public static class IQueryableExtensions
    {
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

//        public static string ToSql<TEntity>(this IQueryable<TEntity> query, DbContext dbCtx)
//        {
//            try
//            {
//                IQueryModelGenerator modelGenerator = dbCtx.GetService<IQueryModelGenerator>();
//                QueryModel queryModel = modelGenerator.ParseQuery(query.Expression);
//                DatabaseDependencies databaseDependencies = dbCtx.GetService<DatabaseDependencies>();
//                QueryCompilationContext queryCompilationContext =
//                    databaseDependencies.QueryCompilationContextFactory.Create(false);
//                RelationalQueryModelVisitor modelVisitor =
//                    (RelationalQueryModelVisitor) queryCompilationContext.CreateQueryModelVisitor();
//                modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
//                var sql = modelVisitor.Queries.First().ToString();
//                return sql;
//            }
//            catch (Exception ex)
//            {
//                return ex.Message;
//            }
//        }
    }
}