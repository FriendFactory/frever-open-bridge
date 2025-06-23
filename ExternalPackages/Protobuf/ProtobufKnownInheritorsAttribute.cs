using System;

namespace Bridge.ExternalPackages.Protobuf
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtobufKnownInheritorsAttribute : Attribute
    {
        public ProtobufKnownInheritorsAttribute(params Type[] knownInheritedTypes)
        {
            KnownInheritedTypes = knownInheritedTypes ?? throw new ArgumentNullException(nameof(knownInheritedTypes));
        }

        public Type[] KnownInheritedTypes { get; private set; }
    }
}