using System.Collections.Generic;
using System.Text;

namespace Bridge.AssetManagerServer
{
    /// <summary>
    /// It allows to build you filters like:
    /// (a > b && b > c) || (d == e || e > 100)
    /// </summary>
    public sealed class ComplexFilterSetup
    {
        private readonly FilterSetup _firstCondition;
        private readonly List<AppendCondition> _appendConditions = new List<AppendCondition>();
        
        public ComplexFilterSetup(FilterSetup filter)
        {
            _firstCondition = filter;
        }

        public ComplexFilterSetup AppendAnd(FilterSetup filter)
        {
            _appendConditions.Add(new AppendCondition(LogicalOperator.And, filter));
            return this;
        }
        

        public ComplexFilterSetup AppendOr(FilterSetup filter)
        {
            _appendConditions.Add(new AppendCondition(LogicalOperator.Or, filter));
            return this;
        }
        
        internal string BuildQuery()
        {
            var stringBuilder = new StringBuilder();
            Append(stringBuilder, _firstCondition);
            foreach (var condition in _appendConditions)
            {
                stringBuilder.AppendFormat("%20{0}%20", condition.LogicalOperator.GetName());
                Append(stringBuilder,condition.FilterSetup);
            }

            return stringBuilder.ToString();
        }

        private static void Append(StringBuilder stringBuilder, FilterSetup filterSetup)
        {
            if (filterSetup.ConditionsCount > 1)
            {
                stringBuilder.AppendFormat("({0})",filterSetup);
            }
            else
            {
                stringBuilder.Append(filterSetup);
            }
        }
        
        private class AppendCondition
        {
            public readonly LogicalOperator LogicalOperator;
            public readonly FilterSetup FilterSetup;

            public AppendCondition(LogicalOperator logicalOperator, FilterSetup filterSetup)
            {
                LogicalOperator = logicalOperator;
                FilterSetup = filterSetup;
            }
        }
    }
}