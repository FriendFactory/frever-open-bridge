using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge.AssetManagerServer
{
    /// <summary>
    /// It allows to build condition chain query like: x.Id > 0 && x.Name.Contains("user") || x.IsPublic
    /// Commands run from right to left
    /// If you want something like: x.Id > 0 && (x.Name.Contains("user") || x.IsPublic) you need to use ComplexFilterSetup
    /// </summary>
    public sealed class FilterSetup
    {
        private const string SENSITVE_FILTER_FORMAT = "{0}({1},'{2}')";
        private const string INSENSITIVE_FILTER_FORMAT = "{0}(tolower({1}),'{2}')";
        
        public string FieldName;
        public FilterType FilterType;
        public object FilterValue;
        public bool CaseSensitive;

        internal int ConditionsCount => _filterChains?.Count+1 ?? 1;

        private List<FilterAppend> _filterChains;

        public FilterSetup()
        {
        }
        
        public FilterSetup(string fieldName, FilterType filterType, object filterValue, bool caseSensitive): this(fieldName, filterType, filterValue)
        {
            CaseSensitive = caseSensitive;
        }
        
        public FilterSetup(string fieldName, FilterType filterType, object filterValue)
        {
            FieldName = fieldName;
            FilterType = filterType;
            FilterValue = filterValue;
        }
        
        private FilterSetup Append(LogicalOperator condOperator, FilterSetup filterSetup)
        {
            if(_filterChains == null)
                _filterChains = new List<FilterAppend>();
            
            _filterChains.Add(new FilterAppend(condOperator, filterSetup));
            return this;
        }
        
        public FilterSetup AppendAnd(FilterSetup filterSetup)
        {
            return Append(LogicalOperator.And, filterSetup);
        }
        
        public FilterSetup AppendOr(FilterSetup filterSetup)
        {
            return Append(LogicalOperator.Or, filterSetup);
        }
        
        public FilterSetup AppendAnd(string fieldName, FilterType filterType, object filterValue, bool caseSensitive = false)
        {
            return Append(LogicalOperator.And, fieldName, filterType, filterValue, caseSensitive);
        }

        public FilterSetup AppendOr(string fieldName, FilterType filterType, object filterValue, bool caseSensitive = false)
        {
            return Append(LogicalOperator.Or, fieldName, filterType, filterValue, caseSensitive);
        }
        
        private FilterSetup Append(LogicalOperator condOperator, string fieldName, FilterType filterType, object filterValue, bool caseSensitive = false)
        {
            var filterSetup = CreateFilterSetup(fieldName, filterType, filterValue, caseSensitive);
            return Append(condOperator, filterSetup);
        }

        private FilterSetup CreateFilterSetup(string fieldName, FilterType filterType, object filterValue,
            bool caseSensitive = false)
        {
            return new FilterSetup(fieldName, filterType, filterValue, caseSensitive);
        }
        
        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(BuildSingleCondition(this));
            if(_filterChains!=null)
                foreach (var filterAppend in _filterChains)
                {
                    var filterStr = BuildSingleCondition(filterAppend.FilterSetup);
                    strBuilder.AppendFormat($"%20{filterAppend.Operator.GetName()}%20{filterStr}");
                }

            return strBuilder.ToString();
        }

        private static string BuildSingleCondition(FilterSetup f)
        {
            var stringBuilder = new StringBuilder();
            
            if (f.FilterType == FilterType.Contains
                || f.FilterType == FilterType.StartWith
                || f.FilterType == FilterType.EndWith)
            {
                var format = f.CaseSensitive ? SENSITVE_FILTER_FORMAT : INSENSITIVE_FILTER_FORMAT;
                if(f.CaseSensitive && !(f.FilterValue is string filterValue)) 
                    throw new Exception("Case sensitive can be applied only for string");

                stringBuilder.AppendFormat(format, f.FilterType.GetName(), f.FieldName,
                    f.CaseSensitive ? f.FilterValue: (f.FilterValue as string)?.ToLower());
                return stringBuilder.ToString();
            }

            stringBuilder.AppendFormat("{0}%20{1}%20", f.FieldName, f.FilterType.GetName());

            if (f.FilterValue is DateTime dt)
            {
                stringBuilder.Append(dt.ToString("yyyy-MM-ddTHH:mm:ss"));
                stringBuilder.Append("Z");
            }
            else if (f.FilterValue is string)
            {
                stringBuilder.AppendFormat("'{0}'", f.FilterValue);
            }
            else if (f.FilterValue is bool boolValue)
            {
                stringBuilder.AppendFormat("{0}", boolValue.ToString().ToLower());
            }
            else
            {
                stringBuilder.Append(f.FilterValue);
            }

            return stringBuilder.ToString();
        }
        
        private class FilterAppend
        {
            public readonly LogicalOperator Operator;
            public readonly FilterSetup FilterSetup;

            public FilterAppend(LogicalOperator @operator, FilterSetup filterSetup)
            {
                Operator = @operator;
                FilterSetup = filterSetup;
            }
        }
    }
}