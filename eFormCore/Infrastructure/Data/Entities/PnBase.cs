using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microting.eForm.Infrastructure.Data.Entities
{
    public abstract class PnBase : BaseEntity
    {
        public async Task Create(MicrotingDbContext dbContext)
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Version = 1;
            WorkflowState = Constants.Constants.WorkflowStates.Created;

            await dbContext.AddAsync(this).ConfigureAwait(false);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            var res = MapVersion(this);
            if (res != null)
            {
                await dbContext.AddAsync(res).ConfigureAwait(false);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Update(MicrotingDbContext dbContext)
        {
            await UpdateInternal(dbContext);
        }

        public async Task Delete(MicrotingDbContext dbContext)
        {
            await UpdateInternal(dbContext, Constants.Constants.WorkflowStates.Removed);
        }

        private async Task UpdateInternal(MicrotingDbContext dbContext, string state = null)
        {
            if (state != null)
            {
                WorkflowState = state;
            }

            if (dbContext.ChangeTracker.HasChanges())
            {
                Version += 1;
                UpdatedAt = DateTime.UtcNow;

                await dbContext.SaveChangesAsync();

                var res = MapVersion(this);
                if (res != null)
                {
                    await dbContext.AddAsync(res).ConfigureAwait(false);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        private object MapVersion(object obj)
        {
            Type type = obj.GetType().UnderlyingSystemType;
            String className = type.Name;
            var name = obj.GetType().FullName + "Version";
            var resultType = Assembly.GetExecutingAssembly().GetType(name);
            if (resultType == null)
                return null;

            var returnObj = Activator.CreateInstance(resultType);

            var curreList = obj.GetType().GetProperties();
            foreach (var prop in curreList)
            {
                if (!prop.PropertyType.FullName.Contains("Microting.eForm.Infrastructure.Data.Entities"))
                {
                    try
                    {
                        var propName = prop.Name;
                        if (propName != "Id")
                        {
                            var propValue = prop.GetValue(obj);
                            Type targetType = returnObj.GetType();
                            PropertyInfo targetProp = targetType.GetProperty(propName);

                            targetProp.SetValue(returnObj, propValue, null);
                        }
                        else
                        {
                            var propValue = prop.GetValue(obj);
                            Type targetType = returnObj.GetType();
                            PropertyInfo targetProp = targetType.GetProperty($"{FirstCharToUpper(className)}Id");

                            targetProp.SetValue(returnObj, propValue, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"{ex.Message} - Property:{prop.Name} probably not found on Class {returnObj.GetType().Name}");
                    }
                }
            }

            return returnObj;
        }

        private static string FirstCharToUpper(string input)
        {
            string pattern = @"_.";
            Regex rg = new Regex(pattern);
            MatchCollection matches = rg.Matches(input);
            if (matches.Any())
            {
                foreach (Match match in matches)
                {
                    input = input.Replace(match.Value, match.Value.Replace("_", "").ToUpper());
                }
            }

            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}