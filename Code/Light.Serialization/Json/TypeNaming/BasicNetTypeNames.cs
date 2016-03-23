using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Light.GuardClauses;

namespace Light.Serialization.Json.TypeNaming
{
    public static class BasicNetTypeNames
    {
        public const string Int32 = "int32";
        public const string Int64 = "int64";
        public const string Int16 = "int16";
        public const string Sbyte = "int8";
        public const string UInt32 = "uint32";
        public const string UInt64 = "uint64";
        public const string UInt16 = "uint16";
        public const string Byte = "byte";
        public const string Single = "float32";
        public const string Double = "float64";
        public const string Decimal = "decimal";
        public const string Bool = "bool";
        public const string Character = "character";
        public const string String = "string";
        public const string Object = "object";
        public const string DateTime = "dateTime";
        public const string TimeSpan = "duration";
        public const string DateTimeOffset = "localDateTime";
        public const string GenericDictionary = "genericMap";
        public const string GenericCollection = "genericCollection";

        public static DomainFriendlyNameMapping AddDefaultMappingsForBasicTypes(this DomainFriendlyNameMapping mapping)
        {
            mapping.MustNotBeNull(nameof(mapping));

            return mapping.AddMapping(Int32, typeof (int))
                          .AddMapping(Int64, typeof(long))
                          .AddMapping(Int16, typeof(short))
                          .AddMapping(Sbyte, typeof(sbyte))
                          .AddMapping(UInt32, typeof(uint))
                          .AddMapping(UInt64, typeof(ulong))
                          .AddMapping(UInt16, typeof(ushort))
                          .AddMapping(Byte, typeof(byte))
                          .AddMapping(Single, typeof(float))
                          .AddMapping(Double, typeof(double))
                          .AddMapping(Decimal, typeof(decimal))
                          .AddMapping(Bool, typeof(bool))
                          .AddMapping(Character, typeof(char))
                          .AddMapping(String, typeof(string))
                          .AddMapping(Object, typeof(object))
                          .AddMapping(DateTime, typeof(DateTime))
                          .AddMapping(TimeSpan, typeof(TimeSpan))
                          .AddMapping(DateTimeOffset, typeof(DateTimeOffset))
                          .AddMapping(GenericCollection, typeof(List<>), typeof(IList<>), typeof(ICollection<>), typeof(IEnumerable<>), typeof(Collection<>), typeof(ObservableCollection<>))
                          .AddMapping(GenericDictionary, typeof(Dictionary<,>), typeof(IDictionary<,>));
        }
    }
}