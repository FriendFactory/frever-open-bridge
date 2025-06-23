using System;
using Bridge.Models.Common;

namespace Bridge.AssetManagerServer
{
    public abstract class QuerySetupBase<T> where T : IEntity
    {
        internal string RootProperty;

        protected QuerySetupBase(string rootProperty)
        {
            if (!HasProperty(rootProperty))
                throw new Exception($"Type {typeof(T).Name} does not have property \"{rootProperty}\"");

            RootProperty = rootProperty;
        }

        protected bool HasProperty(string prop)
        {
            return typeof(T).GetProperty(prop) != null;
        }
    }
}