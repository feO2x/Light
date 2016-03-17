using System;
using System.Collections.Generic;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.TypeNaming;

namespace Light.Serialization.Json.TokenParsers
{
    public class DefaultTokenParsersBuilder
    {
        private ICollectionFactory _collectionFactory = new DefaultGenericCollectionFactory();
        private IDictionaryFactory _dictionaryFactory = new DefaultGenericDictionaryFactory();
        private IInjectableValueNameNormalizer _nameNormalizer = new ToLowerWithoutSpecialCharactersNormalizer();
        private IObjectFactory _objectFactory = new DefaultObjectFactory();
        private ITypeDescriptionProvider _typeDescriptionProvider;
        private ITypeSectionParser _typeSectionParser = new DefaultTypeSectionParser(new SimpleNameToTypeMapping());

        public DefaultTokenParsersBuilder()
        {
            _typeDescriptionProvider = new TypeDescriptionCacheDecorator(new DefaultTypeDescriptionProvider(_nameNormalizer), new Dictionary<Type, TypeCreationDescription>());
        }

        public DefaultTokenParsersBuilder WithObjectFactory(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
            return this;
        }

        public DefaultTokenParsersBuilder WithCollectionFactory(ICollectionFactory collectionFactory)
        {
            _collectionFactory = collectionFactory;
            return this;
        }

        public DefaultTokenParsersBuilder WithDictionaryFactory(IDictionaryFactory dictionaryFactory)
        {
            _dictionaryFactory = dictionaryFactory;
            return this;
        }

        public DefaultTokenParsersBuilder WithTypeSectionParser(ITypeSectionParser parser)
        {
            _typeSectionParser = parser;
            return this;
        }

        public DefaultTokenParsersBuilder ConfigureTypeSectionParser<T>(Action<T> configureParser)
            where T : ITypeSectionParser
        {
            configureParser((T) _typeSectionParser);
            return this;
        }

        public DefaultTokenParsersBuilder WithNameNormalizer(IInjectableValueNameNormalizer nameNormalizer)
        {
            _nameNormalizer = nameNormalizer;

            var typeDescriptionProvider = (_typeDescriptionProvider as TypeDescriptionCacheDecorator)?.DecoratedProvider as DefaultTypeDescriptionProvider;
            if (typeDescriptionProvider != null)
                typeDescriptionProvider.InjectableValueNameNormalizer = nameNormalizer;

            return this;
        }

        public DefaultTokenParsersBuilder WithTypeDescriptionProvider(ITypeDescriptionProvider analyzer)
        {
            _typeDescriptionProvider = analyzer;
            return this;
        }

        public DefaultTokenParsersBuilder WithCacheForDefaultTypeDescriptionProvider(Dictionary<Type, TypeCreationDescription> cache)
        {
            ((TypeDescriptionCacheDecorator) _typeDescriptionProvider).Cache = cache;
            return this;
        }

        public List<IJsonTokenParser> Build()
        {
            return new List<IJsonTokenParser>().AddDefaultTokenParsers(_collectionFactory,
                                                                       _dictionaryFactory,
                                                                       _objectFactory,
                                                                       _typeSectionParser,
                                                                       _nameNormalizer,
                                                                       _typeDescriptionProvider);
        }

        public JsonDeserializerBuilder InjectParsersIntoSerializerBuilder()
        {
            return new JsonDeserializerBuilder(Build());
        }

        public DefaultTokenParsersBuilder EnableDefaultDomainFriendlyNaming(Action<NameToMappingTransformer.IScanningOptions> configureAdditionalTypes = null)
        {
            ConfigureTypeSectionParser<DefaultTypeSectionParser>(p => p.NameToTypeMapping = new DomainFriendlyNameMapping().AddDefaultMappingsForBasicTypes().AddTypes(configureAdditionalTypes));
            return this;
        }
    }
}