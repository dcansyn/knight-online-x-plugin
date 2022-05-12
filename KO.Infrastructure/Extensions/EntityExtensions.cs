using KO.Core.Extensions;
using KO.Core.Models.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace KO.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static string GetTableName(this Type type)
        {
            return type.GetCustomAttribute<TableAttribute>()?.Name;
        }

        public static string GetColumnNames(this Type type)
        {
            var columns = type?.GetProperties()?.Where(x => !Attribute.IsDefined(x, typeof(NotMappedAttribute)))?.Select(x => x.GetCustomAttribute<ColumnAttribute>()?.Name ?? x.Name)?.ToList();
            return string.Join(",", columns);
        }

        public static string GetColumnParameters(this string data)
        {
            return $"@{string.Join(",@", data.Split(','))}";
        }

        public static string GetUpdateColumns(this string data)
        {
            if (!data.Contains(","))
                return $"{data}=@{data}";

            return string.Join(",", data.Split(',').Select(x => $"{x}=@{x}"));
        }

        public static string GetCustomQuery(this Dictionary<string, object> list, params Parameter[] parameters)
        {
            return GetQuery(list, true, parameters);
        }

        public static string GetQuery(this Dictionary<string, object> list, params Parameter[] parameters)
        {
            return GetQuery(list, false, parameters);
        }

        private static string GetQuery(this Dictionary<string, object> list, bool defaultName, params Parameter[] parameters)
        {
            if (parameters.Length <= 0)
                return "NULL IS NULL";

            return string.Join(" AND ", parameters.Where(x => x != null).GroupBy(x => x.ConditionType).Select((conditionType, conditionIndex) =>
            {
                var query = conditionType.Select((parameter, parameterIndex) =>
                {
                    var key = defaultName ? parameter.Name : $"{parameter.Name}_{conditionIndex}_{parameterIndex}";

                    switch (parameter.Operator.Group)
                    {
                        case "NullSyntax":
                            return $"{parameter.Name} {parameter.Operator.DisplayName}";
                        case "Like":
                            list.Add(key, $"%{parameter.Value}%");
                            break;
                        default:
                            list.Add(key, parameter.Value);
                            break;
                    }

                    return $"{parameter.Name} {parameter.Operator.DisplayName} @{key}";
                }).ToList();

                return $"({string.Join($" { conditionType.Key.Get().DisplayName} ", query)})";
            }).ToList());
        }
    }
}
