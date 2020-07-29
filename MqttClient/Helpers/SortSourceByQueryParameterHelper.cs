using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MqttClient.Helpers
{
    public class SortSourceByQueryParameterHelper<TSource>
    {
        private const int ASC = 1;
        private const int DESC = -1;

        private Dictionary<string, int> GetOrdersByParameterName(string queryParameter)
        {
            var parameterNames = queryParameter.Split(',');
            Dictionary<string, int> ordersByParameterName = new Dictionary<string, int>();

            foreach (var paramName in parameterNames)
            {
                if (paramName.First() == '-')
                {
                    ordersByParameterName[paramName.TrimStart('-').ToLower()] = DESC;
                }
                else
                {
                    ordersByParameterName[paramName] = ASC;
                }
            }

            return ordersByParameterName;
        }

        public IEnumerable<TSource> Sort(IEnumerable<TSource> source, string queryParameter)
        {
            if (queryParameter is null)
            {
                return source;
            }

            var ordersByParameterName = GetOrdersByParameterName(queryParameter);

            IOrderedEnumerable<TSource> orderedEnumerable = null;

            var sourceProps = typeof(TSource)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var attributesJson = sourceProps
                .Select(p => p.GetCustomAttribute<JsonPropertyNameAttribute>())
                .Where(p => p != null)
                .Select(p => p.Name).ToList();

            foreach (var orderByParamName in ordersByParameterName)
            {
                if (attributesJson.Contains(orderByParamName.Key))
                {
                    var propToOrder = sourceProps
                        .Single(p =>
                            p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name == orderByParamName.Key);

                    if (orderedEnumerable is null)
                    {
                        orderedEnumerable = orderByParamName.Value == ASC
                            ? source.OrderBy(j => propToOrder.GetValue(j))
                            : source.OrderByDescending(j => propToOrder.GetValue(j));
                    }
                    else
                    {
                        orderedEnumerable = orderByParamName.Value == ASC
                            ? orderedEnumerable.ThenBy(j => propToOrder.GetValue(j))
                            : orderedEnumerable.ThenByDescending(j => propToOrder.GetValue(j));
                    }
                }
            }

            return orderedEnumerable ?? source;
        }
    }
}
