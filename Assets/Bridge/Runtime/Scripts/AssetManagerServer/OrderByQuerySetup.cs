using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Models.Common;

namespace Bridge.AssetManagerServer
{
    public class OrderByQuerySetup<T>:QuerySetupBase<T> where T:IEntity
    {
        internal List<string> DeepPropertyPath;
        internal OrderByType OrderByType;
        
        public OrderByQuerySetup(string rootProperty, OrderByType orderByType) : base(rootProperty)
        {
            OrderByType = orderByType;
        }

        public OrderByQuerySetup(string[] propertyPath, OrderByType orderByType): base(propertyPath.First())
        {
            if(propertyPath==null)
                throw new InvalidOperationException("Property path for ordering must not be null");
            OrderByType = orderByType;

            DeepPropertyPath = propertyPath.Skip(1).ToList();
        }
    }
}