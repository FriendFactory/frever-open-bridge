using System.Collections.Generic;

namespace Bridge.AssetManagerServer
{
    internal static class Extensions
    {
        private static readonly Dictionary<FilterType, string> ServerFilters = new Dictionary<FilterType, string>()
        {
            {FilterType.Contains, "contains"},
            {FilterType.EndWith, "endswith"},
            {FilterType.StartWith,"startswith" },
            {FilterType.Equals, "eq"},
            {FilterType.GreatThan,"gt" },
            {FilterType.LessThan, "lt"},
            {FilterType.GreatThanOrEquals, "ge"},
            {FilterType.LessThanOrEquals, "le"},

        };

        private static readonly Dictionary<LogicalOperator, string> OperatorNames =
            new Dictionary<LogicalOperator, string>()
            {
                {LogicalOperator.Or, "or"},
                {LogicalOperator.And, "and"}
            };

        public static string GetName(this FilterType filterType)
        {
            return ServerFilters[filterType];
        }

        private static Dictionary<OrderByType, string> _serverOrders = new Dictionary<OrderByType, string>()
        {
            {OrderByType.Ascend, "asc"},
            {OrderByType.Descend, "desc"}
        };

        public static string GetName(this OrderByType filterType)
        {
            return _serverOrders[filterType];
        }

        public static string GetName(this LogicalOperator logicalOperator)
        {
            return OperatorNames[logicalOperator];
        }
    }
}