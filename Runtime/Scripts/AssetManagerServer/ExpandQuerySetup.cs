using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Models.Common;

namespace Bridge.AssetManagerServer
{
    public class ExpandQuerySetup<T> : QuerySetupBase<T> where T:IEntity
    {
        internal bool IsDeepLevel => ThenInclude != null && ThenInclude.Count > 0;
        internal List<string> ThenInclude { get; private set; }

        public ExpandQuerySetup(string rootProperty) : base(rootProperty)
        {
        }

        public ExpandQuerySetup(string[] propFullHierarchy): base(propFullHierarchy.First())
        {
            if(propFullHierarchy==null || propFullHierarchy.Length ==0)
                throw new Exception("Hierarchy must not be null or empty");

            var rootProp = propFullHierarchy.First();
            if (!HasProperty(rootProp))
                throw new Exception($"Type {typeof(T).Name} does not have property \"{rootProp}\"");

            RootProperty = rootProp;
            if (propFullHierarchy.Length > 1)
            {
                ThenInclude = propFullHierarchy.Skip(1).ToList();
            }
        }

        public ExpandQuerySetup<T> ThenExpand(string next)
        {
            if(ThenInclude == null)
                ThenInclude = new List<string>();

            ThenInclude.Add(next);
            return this;
        }
    }
}