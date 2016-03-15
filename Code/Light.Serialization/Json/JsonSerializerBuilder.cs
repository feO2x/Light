using System;
using System.Collections.Generic;
using System.Linq;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.LowLevelWriting;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.SerializationRules;
using Light.Serialization.Json.WriterInstructors;

namespace Light.Serialization.Json
{
    public class JsonSerializerBuilder
    {
        private readonly List<Rule> _rules = new List<Rule>();
        private readonly List<IJsonWriterInstructor> _writerInstructors;
        private IDictionary<Type, IJsonWriterInstructor> _instructorCache;
        private IReadableValuesTypeAnalyzer _typeAnalyzer = new ValueProvidersCacheDecorator(new PublicPropertiesAndFieldsAnalyzer(), new Dictionary<Type, IList<IValueProvider>>());
        private IJsonWriterFactory _writerFactory;
        private ICharacterEscaper _characterEscaper = new DefaultCharacterEscaper();
        private Func<IList<IJsonWriterInstructor>> _createList = CreateDefaultList;

        private static IList<IJsonWriterInstructor> CreateDefaultList()
        {
            return new List<IJsonWriterInstructor>();
        } 

        public JsonSerializerBuilder()
        {
            UseDefaultWriterFactory();
            _instructorCache = new Dictionary<Type, IJsonWriterInstructor>();

            _writerInstructors = new List<IJsonWriterInstructor>()
                .AddDefaultWriterInstructors(new List<IPrimitiveTypeFormatter>().AddDefaultPrimitiveTypeFormatters(_characterEscaper)
                                                                                .ToDictionary(f => f.TargetType),
                                             _typeAnalyzer);
        }

        public JsonSerializerBuilder WithWriterFactory(IJsonWriterFactory writerFactory)
        {
            _writerFactory = writerFactory;
            return this;
        }

        public JsonSerializerBuilder WithListCreationFunction(Func<IList<IJsonWriterInstructor>> createList)
        {
            _createList = createList;
            return this;
        }

        public JsonSerializerBuilder WithCharacterEscaper(ICharacterEscaper characterEscaper)
        {
            _characterEscaper = characterEscaper;

            ConfigureFormatterOfPrimitiveTypeInstructor<CharFormatter>(f => f.CharacterEscaper = characterEscaper);
            ConfigureFormatterOfPrimitiveTypeInstructor<StringFormatter>(f => f.CharacterEscaper = characterEscaper);

            return this;
        }

        public JsonSerializerBuilder WithTypeAnalyzer(IReadableValuesTypeAnalyzer typeAnalyzer)
        {
            _typeAnalyzer = typeAnalyzer;

            var complexObjectInstructor = _writerInstructors.OfType<ComplexObjectInstructor>().FirstOrDefault();
            if (complexObjectInstructor != null)
                complexObjectInstructor.TypeAnalyzer = _typeAnalyzer;

            return this;
        }

        public JsonSerializerBuilder UseDefaultWriterFactory()
        {
            _writerFactory = new JsonWriterFactory();
            return this;
        }

        public JsonSerializerBuilder ConfigureDefaultWriterFactory(Action<JsonWriterFactory> configureFactory)
        {
            configureFactory((JsonWriterFactory) _writerFactory);
            return this;
        }

        public JsonSerializerBuilder WithInstructorCache(IDictionary<Type, IJsonWriterInstructor> instructorCache)
        {
            _instructorCache = instructorCache;
            return this;
        }

        public JsonSerializerBuilder ConfigureInstructor<T>(Action<T> configureInstructor)
            where T : IJsonWriterInstructor
        {
            configureInstructor(_writerInstructors.OfType<T>().First());
            return this;
        }

        public JsonSerializerBuilder ConfigureFormatterOfPrimitiveTypeInstructor<T>(Action<T> configureFormatter)
            where T : IPrimitiveTypeFormatter
        {
            configureFormatter(_writerInstructors.OfType<PrimitiveTypeInstructor>()
                                                 .First()
                                                 .PrimitiveTypeToFormattersMapping
                                                 .Values
                                                 .OfType<T>()
                                                 .First());
            return this;
        }

        public JsonSerializerBuilder WithRuleFor<T>(Action<Rule<T>> configureRule)
        {
            var targetType = typeof (T);
            var targetRule = (Rule<T>) _rules.FirstOrDefault(r => r.TargetType == targetType);
            if (targetRule == null)
            {
                targetRule = new Rule<T>(_typeAnalyzer);
                _rules.Add(targetRule);
            }

            configureRule(targetRule);
            return this;
        }

        public ISerializer Build()
        {
            var writerInstructors = _createList();
            foreach (var rule in _rules)
            {
                writerInstructors.Add(rule.CreateInstructor());
            }
            foreach (var instructor in _writerInstructors)
            {
                writerInstructors.Add(instructor);
            }
            return new JsonSerializer((IReadOnlyList<IJsonWriterInstructor>)writerInstructors, _writerFactory, _instructorCache);
        }
    }
}