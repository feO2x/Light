using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;
using Light.Serialization.Json;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.LowLevelWriting;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.TokenParsers;
using Light.Serialization.Json.WriterInstructors;
using Microsoft.Practices.Unity;

namespace Light.Serialization.UnityContainerIntegration
{
    public static class LightSerializationModule
    {
        public static IUnityContainer RegisterDefaultDeserializationTypes(this IUnityContainer container)
        {
            container.MustNotBeNull(nameof(container));

            return container.RegisterType<IDeserializer, JsonDeserializer>()
                            .RegisterType<IJsonReaderFactory, SingleBufferJsonReaderFactory>()
                            .RegisterType<JsonReaderSymbols>(new PerResolveLifetimeManager())
                            .RegisterType<IReadOnlyList<IJsonTokenParser>, IJsonTokenParser[]>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, IntParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, DateTimeParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, TimeSpanParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, UIntParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, ShortParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, UShortParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, ByteParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, SByteParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, LongParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, StringParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, DoubleParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, NullParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, CharacterParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, BooleanParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, ArrayToGenericCollectionParser>()
                            .RegisterTypeWithTypeName<IJsonTokenParser, ComplexTypeParser>()
                            .RegisterType<ICollectionFactory, DefaultGenericCollectionFactory>()
                            .RegisterType<IObjectFactory, UnityObjectFactory>()
                            .RegisterType<INameToTypeMapping, SimpleNameToTypeMapping>()
                            .RegisterType<IInjectableValueNameNormalizer, ToLowerWithoutSpecialCharactersNormalizer>()
                            .RegisterType<ITypeDescriptionProvider, DefaultTypeDescriptionProvider>();
        }

        public static IUnityContainer RegisterDefaultSerializationTypes(this IUnityContainer container)
        {
            container.MustNotBeNull(nameof(container));

            return container.RegisterType<ISerializer, JsonSerializer>()
                            .RegisterType<IJsonWriterFactory, JsonWriterFactory>()
                            .RegisterType<IDictionary<Type, IJsonWriterInstructor>>(new ContainerControlledLifetimeManager(), new InjectionFactory(c => new Dictionary<Type, IJsonWriterInstructor>()))
                            .RegisterType<IReadOnlyList<IJsonWriterInstructor>, IJsonWriterInstructor[]>()
                            .RegisterTypeWithTypeName<IJsonWriterInstructor, PrimitiveWriterInstructor>()
                            .RegisterTypeWithTypeName<IJsonWriterInstructor, EnumerationToStringInstructor>()
                            .RegisterTypeWithTypeName<IJsonWriterInstructor, DictionaryInstructor>()
                            .RegisterTypeWithTypeName<IJsonWriterInstructor, CollectionInstructor>()
                            .RegisterTypeWithTypeName<IJsonWriterInstructor, ComplexObjectInstructor>()
                            .RegisterType<IDictionary<Type, IPrimitiveTypeFormatter>>(new ContainerControlledLifetimeManager(),
                                                                                      new InjectionFactory(c => c.ResolveAll<IPrimitiveTypeFormatter>().ToDictionary(f => f.TargetType)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.IntFormatter, new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<int>(false)))
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, StringFormatter>()
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, DoubleFormatter>()
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, TimeSpanFormatter>()
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.GuidFormatter, new InjectionFactory(c => new ToStringWithQuotationMarksFormatter<Guid>(false)))
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, BooleanFormatter>()
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, DecimalFormatter>()
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.LongFormatter, new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<long>(false)))
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, FloatFormatter>()
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, CharFormatter>()
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.ShortFormatter, new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<short>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.ByteFormatter, new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<byte>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.UIntFormatter, new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<uint>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.ULongFormatter, new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<ulong>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.UShortFormatter, new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<ushort>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.SByteFormatter, new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<sbyte>(false)))
                            .RegisterType<ICharacterEscaper>(new PerResolveLifetimeManager(), new InjectionFactory(c => new DefaultCharacterEscaper()))
                            .RegisterType<IReadableValuesTypeAnalyzer>(new InjectionFactory(c => new ValueProvidersCacheDecorator(new PublicPropertiesAndFieldsAnalyzer(), new Dictionary<Type, IList<IValueProvider>>())));
        }

        public static IUnityContainer RegisterTypeWithTypeName<TFrom, TTo>(this IUnityContainer container, LifetimeManager lifetimeManager = null) where TTo : TFrom
        {
            container.MustNotBeNull(nameof(container));

            lifetimeManager = lifetimeManager ?? new TransientLifetimeManager();
            return container.RegisterType<TFrom, TTo>(typeof (TTo).Name, lifetimeManager);
        }
    }
}