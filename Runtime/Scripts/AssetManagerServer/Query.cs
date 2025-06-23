using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bridge.Models.Common;
using UnityEngine;

namespace Bridge.AssetManagerServer
{
    /// <summary>
    /// Class for setup custom query. Please use 'nameof(SomeClass.FieldName)' as input parameters in methods;
    /// Each property represent true name of some field 
    /// </summary>
    public class Query<T> where T: IEntity
    {
        public const string ALL_FIELDS_SIGN = "*";

        public string[] SelectedFieldsName { get; private set; }
        public int? MaxTop;
        public int? Skip;

        private delegate bool QueryBuilding(ref StringBuilder s);

        private readonly QueryBuilding[] _buildings;
        private FilterSetup _filter;
        private ComplexFilterSetup _complexFilter;

        private readonly List<ExpandQuerySetup<T>> _expandQueryBuilders = new List<ExpandQuerySetup<T>>();
        private OrderByQuerySetup<T> _orderByQuerySetup;
        
        private const char AND_SIGN = '&';

        public Query()
        {
            _buildings = new[]
            {
                AppendSelectedFields,
                AppendOrderBy,
                AppendFilters,
                AppendExpands,
                AppendTopMax,
                new QueryBuilding(AppendSkip),
            };
        }

        public void SetOrderBy(string fieldName, OrderByType type)
        {
            _orderByQuerySetup = new OrderByQuerySetup<T>(fieldName, type);
        }
        
        public void SetDeepOrderBy(string[] fieldHierarchyPath, OrderByType type)
        {
            _orderByQuerySetup = new OrderByQuerySetup<T>(fieldHierarchyPath, type);
        }

        public void SetSelectedFieldsNames(params string[] fieldNames)
        {
            if (fieldNames == null || fieldNames.Length == 0)
            {
                Debug.LogWarning("Trying to set NULL or EMPTY selected fields list");
                return;
            }

            if (fieldNames.All(HasProperty) || (fieldNames.Length == 1 && fieldNames[0]== ALL_FIELDS_SIGN))
            {
                SelectedFieldsName = fieldNames;
            }
            else
            {
                Debug.LogError("Can't create query for type " + typeof(T) + " . Field " + fieldNames.FirstOrDefault(x=> !HasProperty(x)) + " does not exist");
            }
        }

        public ExpandQuerySetup<T> ExpandField(string fieldName)
        {
            if(!HasProperty(fieldName))
                throw new Exception($"Type {typeof(T).Name} has no property {fieldName}");

            var firstDeepLevelBuilder =
                _expandQueryBuilders.FirstOrDefault(x => !x.IsDeepLevel && x.RootProperty == fieldName);
            if (firstDeepLevelBuilder!=null)
            {
                return firstDeepLevelBuilder;
            }

            var builder = new ExpandQuerySetup<T>(fieldName);
            _expandQueryBuilders.Add(builder);
            return builder;
        }

        public void ExpandField(ExpandQuerySetup<T> setup)
        {
            _expandQueryBuilders.Add(setup);
        }

        public void ExpandFields(params ExpandQuerySetup<T>[] setups)
        {
            _expandQueryBuilders.AddRange(setups);
        }

        public void SetExpandFieldsNames(params string[] fieldNames)
        {
            if (fieldNames == null || fieldNames.Length == 0)
            {
                throw new Exception("Trying set NULL or Empty fields names list for expanding filter");
            }

            foreach (var fieldName in fieldNames)
            {
                if(_expandQueryBuilders.Any(x=>x.RootProperty == fieldName))
                    continue;

                _expandQueryBuilders.Add(new ExpandQuerySetup<T>(fieldName));
            }
        }
        
        public void SetFilters(params FilterSetup[] filters)
        {
            if (filters == null || filters.Length == 0)
            {
                Debug.LogWarning("Trying to set NULL or Empty filter list");
                return;
            }

            if (filters.All(x=> HasProperty(x.FieldName)))
            {
                var rootFilter = filters[0];
                for (int i = 1; i < filters.Length; i++)
                {
                    rootFilter.AppendAnd(filters[i]);
                }
                _filter = rootFilter;
            }
            else
            {
                Debug.LogError("Can't create query for type " + typeof(T) + " . Field " + filters.FirstOrDefault(x => !HasProperty(x.FieldName)) + " does not exist");
            }
        }

        public void SetFilters(ComplexFilterSetup complexFilter)
        {
            _complexFilter = complexFilter;
        }

        public void SetMaxTop(int val)
        {
            MaxTop = val;
        }
        
        public void SetSkip(int val)
        {
            val = Mathf.Max(val, 0);
            Skip = val;
        }

        private bool HasProperty(string prop)
        {
            return typeof(T).GetProperty(prop) != null;
        }

        internal virtual string BuildQuery()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('?');
            for (int i = 0; i < _buildings.Length; i++)
            {
                var addedFilter = _buildings[i].Invoke(ref stringBuilder);
                if (addedFilter && _buildings.Length!=-1)
                {
                    stringBuilder.Append(AND_SIGN);
                }
            }
            
            var queryStr = stringBuilder.ToString();
            if (queryStr.EndsWith(AND_SIGN.ToString()))
                queryStr = queryStr.TrimEnd(AND_SIGN);
            return queryStr;
        }

        private bool AppendSelectedFields(ref StringBuilder stringBuilder)
        {
            if (SelectedFieldsName == null)
                return false;

            stringBuilder.Append("$select=");
            for (var i = 0; i < SelectedFieldsName.Length; i++)
            {
                var s = SelectedFieldsName[i];
                stringBuilder.Append(s);
                if (i != SelectedFieldsName.Length - 1)
                    stringBuilder.Append(',');
            }

            return true;
        }

        private bool AppendOrderBy(ref StringBuilder stringBuilder)
        {
            if(_orderByQuerySetup==null)
                return false;

            var argumentQuery = new StringBuilder(_orderByQuerySetup.RootProperty);
            if(_orderByQuerySetup.DeepPropertyPath!=null)
                foreach (var fieldName in _orderByQuerySetup.DeepPropertyPath)
                {
                    argumentQuery.AppendFormat("/{0}", fieldName);
                }
            
            stringBuilder.AppendFormat("$OrderBy={0}%20{1}", argumentQuery, _orderByQuerySetup.OrderByType.GetName());
            return true;
        }

        private bool AppendFilters(ref StringBuilder stringBuilder)
        {
            if (_filter == null && _complexFilter == null)
                return false;

            if (_filter != null && _complexFilter !=null)
            {
                throw new Exception("Please user deprecated filters OR new filter builder, but not together");
            }
            
            stringBuilder.Append("$Filter=");
            if (_filter != null)
            {
                stringBuilder.Append(_filter);
                return true;
            }

            var queryString = _complexFilter.BuildQuery();
            stringBuilder.Append(queryString);
            return true;
        }

        private bool AppendExpands(ref StringBuilder stringBuilder)
        {
            if (_expandQueryBuilders == null || _expandQueryBuilders.Count == 0)
                return false;

            var builder = new ExpandDeepQueryBuilder<T>();
            var setups =  _expandQueryBuilders.ToArray();
            var addedExpandKeyword = false;

            foreach (var setup in setups)
            {
                var res = builder.Build(setup,!addedExpandKeyword);
                stringBuilder.Append(res);
                addedExpandKeyword = true;
            }

            return true;
        }


        private bool AppendTopMax(ref StringBuilder stringBuilder)
        {
            if (MaxTop.HasValue)
            {
                stringBuilder.AppendFormat("$top={0}", MaxTop.Value);
                return true;
            }

            return false;
        }
        
        private bool AppendSkip(ref StringBuilder stringBuilder)
        {
            if (Skip.HasValue)
            {
                stringBuilder.AppendFormat("$skip={0}",Skip.Value);
                return true;
            }

            return false;
        }
    }
}