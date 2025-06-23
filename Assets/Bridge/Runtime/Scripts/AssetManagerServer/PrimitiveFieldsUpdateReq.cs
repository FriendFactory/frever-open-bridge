using System;
using System.Collections.Generic;
using System.Linq;
using Bridge.Models.Common;

namespace Bridge.AssetManagerServer
{
    public sealed class PrimitiveFieldsUpdateReq<T> : OptimizedUpdateReqBase<T> where T: IEntity
    {
        private static Type[] PRIMITIVE_TYPES = {typeof(string), typeof(double), typeof(DateTime)};
        
        internal override T OriginModel => _target;
        internal override T TargetModel => _target;
        public override bool HasDataToUpdate => _updating.Count > 0;

        private readonly IDictionary<string, object> _updating = new Dictionary<string, object>();

        private readonly T _target;
        
        public PrimitiveFieldsUpdateReq(T target)
        {
            _target = target;
        }

        public void UpdateProperty(string propertyName)
        {
            var p = _target.GetType().GetProperty(propertyName);
            if (p == null)
            {
                throw new Exception($"Can't find property: {propertyName}");
            }

            var isPrimitive = IsPrimitive(p.PropertyType) || IsNullablePrimitive(p.PropertyType);
            if (!isPrimitive)
            {
                throw new Exception($"You can update only primitive property. Property {propertyName} is not primitive");
            }

            _updating.Add(propertyName, p.GetValue(_target));
        }

        private bool IsNullablePrimitive(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType != null && IsPrimitive(underlyingType);
        }

        private bool IsPrimitive(Type type)
        {
            return type.IsPrimitive || PRIMITIVE_TYPES.Contains(type);
        }        
        protected override object BuildQueryObjectInternal(bool withFilesData)
        {
            if (!_updating.ContainsKey(nameof(IEntity.Id)))
            {
                _updating.Add(nameof(IEntity.Id), _target.Id);
            }

            return _updating;
        }
    }
}