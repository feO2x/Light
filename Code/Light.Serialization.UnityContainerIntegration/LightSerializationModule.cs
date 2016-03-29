﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Light.GuardClauses;
using Light.Serialization.Json;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.LowLevelWriting;
using Light.Serialization.Json.ObjectReferencePreservation;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.TokenParsers;
using Light.Serialization.Json.TypeNaming;
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
                            .RegisterType<IJsonReaderFactory, SingleBufferJsonReaderFactory>(new ContainerControlledLifetimeManager())
                            .RegisterType<IReadOnlyList<IJsonTokenParser>, IJsonTokenParser[]>()
                            .RegisterType<Dictionary<JsonTokenTypeCombination, IJsonTokenParser>>(new ContainerControlledLifetimeManager(),
                                                                                                  new InjectionFactory(c => new Dictionary<JsonTokenTypeCombination, IJsonTokenParser>()))
                            .RegisterTypeWithTypeName<IJsonTokenParser, UnsignedIntegerParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, SignedIntegerParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, DateTimeParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonStringToPrimitiveParser, DateTimeParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, TimeSpanParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonStringToPrimitiveParser, TimeSpanParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, DateTimeOffsetParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, FloatParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, DecimalParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, DoubleParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, BooleanParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, NullParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, CharacterParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, EnumerationValueParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, StringParser>(new ContainerControlledLifetimeManager())
                            .RegisterType<IJsonTokenParser>(KnownNames.JsonStringParserOrchestrator,
                                                            new InjectionFactory(c => new JsonStringInheritenceParser(c.Resolve<IJsonStringToPrimitiveParser[]>(),
                                                                                                                       c.Resolve<StringParser>())))
                            .RegisterTypeWithTypeName<IJsonTokenParser, GenericDictionaryParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, ArrayToGenericCollectionParser>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonTokenParser, ComplexObjectParser>(new ContainerControlledLifetimeManager())
                            .RegisterType<ICollectionFactory, DefaultGenericCollectionFactory>(new ContainerControlledLifetimeManager())
                            .RegisterType<IDictionaryFactory, DefaultGenericDictionaryFactory>(new ContainerControlledLifetimeManager())
                            .RegisterType<IObjectFactory, UnityObjectFactory>(new ContainerControlledLifetimeManager())
                            .RegisterType<INameToTypeMapping, SimpleNameToTypeMapping>(new ContainerControlledLifetimeManager())
                            .RegisterType<ITypeSectionParser, DefaultTypeSectionParser>(new ContainerControlledLifetimeManager())
                            .RegisterType<IIdentifierParser, DefaultIdentifierParser>(new ContainerControlledLifetimeManager())
                            .RegisterType<IReferenceParser, DefaultReferenceParser>(new ContainerControlledLifetimeManager())
                            .RegisterType<IInjectableValueNameNormalizer, ToLowerWithoutSpecialCharactersNormalizer>(new ContainerControlledLifetimeManager())
                            .RegisterType<ITypeDescriptionProvider, DefaultTypeDescriptionProvider>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer RegisterDefaultSerializationTypes(this IUnityContainer container)
        {
            container.MustNotBeNull(nameof(container));

            return container.RegisterType<ISerializer, JsonSerializer>()
                            .RegisterType<IJsonWriterFactory, JsonWriterFactory>(new ContainerControlledLifetimeManager())
                            .RegisterType<IDictionary<Type, IJsonWriterInstructor>>(new ContainerControlledLifetimeManager(),
                                                                                    new InjectionFactory(c => new Dictionary<Type, IJsonWriterInstructor>()))
                            .RegisterType<IReadOnlyList<IJsonWriterInstructor>, IJsonWriterInstructor[]>()
                            .RegisterTypeWithTypeName<IJsonWriterInstructor, PrimitiveTypeInstructor>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IJsonWriterInstructor, EnumerationToStringInstructor>(new ContainerControlledLifetimeManager())
                            //.RegisterTypeWithTypeName<IJsonWriterInstructor, DictionaryInstructor>(new ContainerControlledLifetimeManager())
                            .RegisterType<IJsonWriterInstructor, PreserveObjectReferencesDecorator>(typeof(DictionaryInstructor).Name,
                            new ContainerControlledLifetimeManager(),
                                                                                            new InjectionFactory(c => new PreserveObjectReferencesDecorator(
                                                                                                new DictionaryInstructor(c.Resolve<IDictionary<Type, IPrimitiveTypeFormatter>>()),
                                                                                                c.Resolve<ObjectReferencePreserver>())))
                            .RegisterTypeWithTypeName<IJsonWriterInstructor, CollectionInstructor>(new ContainerControlledLifetimeManager())
                            .RegisterType<IJsonWriterInstructor, PreserveObjectReferencesDecorator>(typeof(ComplexObjectInstructor).Name,
                            new ContainerControlledLifetimeManager(),
                                                                                            new InjectionFactory(c => new PreserveObjectReferencesDecorator(
                                                                                                new ComplexObjectInstructor(c.Resolve<IReadableValuesTypeAnalyzer>()),
                                                                                                c.Resolve<ObjectReferencePreserver>())))
                            .RegisterType<IDictionary<Type, IPrimitiveTypeFormatter>>(new ContainerControlledLifetimeManager(),
                                                                                      new InjectionFactory(c => c.ResolveAll<IPrimitiveTypeFormatter>().ToDictionary(f => f.TargetType)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.IntFormatter, new ContainerControlledLifetimeManager(),
                                                                   new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<int>(false)))
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, StringFormatter>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, DoubleFormatter>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, DateTimeFormatter>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, DateTimeOffsetFormatter>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, TimeSpanFormatter>(new ContainerControlledLifetimeManager())
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.GuidFormatter,
                                                                   new ContainerControlledLifetimeManager(),
                                                                   new InjectionFactory(c => new ToStringWithQuotationMarksFormatter<Guid>(false)))
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, BooleanFormatter>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, DecimalFormatter>(new ContainerControlledLifetimeManager())
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.LongFormatter,
                                                                   new ContainerControlledLifetimeManager(),
                                                                   new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<long>(false)))
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, FloatFormatter>(new ContainerControlledLifetimeManager())
                            .RegisterTypeWithTypeName<IPrimitiveTypeFormatter, CharFormatter>(new ContainerControlledLifetimeManager())
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.ShortFormatter,
                                                                   new ContainerControlledLifetimeManager(),
                                                                   new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<short>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.ByteFormatter,
                                                                   new ContainerControlledLifetimeManager(),
                                                                   new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<byte>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.UIntFormatter,
                                                                   new ContainerControlledLifetimeManager(),
                                                                   new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<uint>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.ULongFormatter,
                                                                   new ContainerControlledLifetimeManager(),
                                                                   new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<ulong>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.UShortFormatter,
                                                                   new ContainerControlledLifetimeManager(),
                                                                   new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<ushort>(false)))
                            .RegisterType<IPrimitiveTypeFormatter>(KnownNames.SByteFormatter,
                                                                   new ContainerControlledLifetimeManager(),
                                                                   new InjectionFactory(c => new ToStringPrimitiveTypeFormatter<sbyte>(false)))
                            .RegisterType<ICharacterEscaper>(new ContainerControlledLifetimeManager(),
                                                             new InjectionFactory(c => new DefaultCharacterEscaper()))
                            .RegisterType<IReadableValuesTypeAnalyzer>(new ContainerControlledLifetimeManager(),
                                                                       new InjectionFactory(c => new ValueProvidersCacheDecorator(new PublicPropertiesAndFieldsAnalyzer(),
                                                                                                                                  new Dictionary<Type, IList<IValueProvider>>())))
                            .RegisterInstance(new ObjectReferencePreserver(
                                                                            new Dictionary<object, uint>()),
                                                                        new ContainerControlledLifetimeManager())
                            .RegisterType<IJsonWriterFactory, JsonWriterFactory>();
        }

        public static IUnityContainer RegisterIndentingWhitespaceformatter(this IUnityContainer container)
        {
            container.MustNotBeNull(nameof(container));

            return container.RegisterType<IJsonWriterFactory, JsonWriterFactory>(new InjectionFactory(c => new JsonWriterFactory
                                                                                                           {
                                                                                                               JsonWhitespaceFormatter = new IndentingWhitespaceFormatter()
                                                                                                           }));
        }

        public static IUnityContainer RegisterTypeWithTypeName<TFrom, TTo>(this IUnityContainer container,
                                                                           LifetimeManager lifetimeManager = null) where TTo : TFrom
        {
            container.MustNotBeNull(nameof(container));

            lifetimeManager = lifetimeManager ?? new TransientLifetimeManager();
            return container.RegisterType<TFrom, TTo>(typeof (TTo).Name, lifetimeManager);
        }

        public static IUnityContainer UseDomainFriendlyNames(this IUnityContainer container, Action<NameToMappingTransformer.IScanningOptions> configureTypes = null)
        {
            container.MustNotBeNull(nameof(container));

            var domainFriendlyNameMapping = new DomainFriendlyNameMapping();
            container.RegisterInstance<INameToTypeMapping>(domainFriendlyNameMapping);
            container.RegisterInstance<ITypeToNameMapping>(domainFriendlyNameMapping);

            if (configureTypes != null)
                domainFriendlyNameMapping.AddTypes(configureTypes);

            return container;
        }

        public static IUnityContainer UseDefaultDomainFriendlyNames(this IUnityContainer container, Action<NameToMappingTransformer.IScanningOptions> configureAddtionalTypes = null)
        {
            container.MustNotBeNull(nameof(container));

            var domainFriendlyNameMapping = new DomainFriendlyNameMapping().AddDefaultMappingsForBasicTypes();
            container.RegisterInstance<INameToTypeMapping>(domainFriendlyNameMapping);
            container.RegisterInstance<ITypeToNameMapping>(domainFriendlyNameMapping);

            if (configureAddtionalTypes != null)
                domainFriendlyNameMapping.AddTypes(configureAddtionalTypes);

            return container;
        }

        public static IUnityContainer ConfigureAdditionalDomainFriendlyNames(this IUnityContainer container, Action<NameToMappingTransformer.IScanningOptions> configureTypes)
        {
            container.MustNotBeNull(nameof(container));

            container.Resolve<DomainFriendlyNameMapping>().AddTypes(configureTypes);

            return container;
        }
    }
}