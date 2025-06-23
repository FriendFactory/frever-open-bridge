using System;

namespace Bridge.ExternalPackages.Protobuf
{
    /// <summary>
    ///     Manages the order of the property in auto-generated protobuf contract.
    ///     Use it on adding new property to published contracts with any Order greater than any existing attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ProtoNewFieldAttribute : Attribute
    {
        public ProtoNewFieldAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }
}