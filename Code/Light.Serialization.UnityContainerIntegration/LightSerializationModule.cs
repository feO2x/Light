using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.TokenParsers;
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

        public static IUnityContainer RegisterTypeWithTypeName<TFrom, TTo>(this IUnityContainer container, LifetimeManager lifetimeManager = null) where TTo : TFrom
        {
            container.MustNotBeNull(nameof(container));

            lifetimeManager = lifetimeManager ?? new TransientLifetimeManager();
            return container.RegisterType<TFrom, TTo>(typeof (TTo).Name, lifetimeManager);
        }
    }
}