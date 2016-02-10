using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.LowLevelWriting;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.SerializationRules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Serialization.Json
{
    public class JsonSerializerBuilder
    {
        private Func<IList<IJsonWriterInstructor>> _createWriterInstructorList = CreateList;
        private IList<IJsonWriterInstructor> _defaultWriterInstructors;
        private IDictionary<Type, IJsonWriterInstructor> _instructorCache;
        private readonly List<Rule> _rules = new List<Rule>();
        private IJsonWriterFactory _writerFactory;
        private IReadableValuesTypeAnalyzer _readableValuesTypeAnalyzer;

        public JsonSerializerBuilder()
        {
            var characterEscaper = new DefaultCharacterEscaper();
            var primitiveTypeToFormattersMapping = new List<IPrimitiveTypeFormatter>().AddDefaultPrimitiveTypeFormatters(characterEscaper)
                                                                                      .ToDictionary(f => f.TargetType);

            _readableValuesTypeAnalyzer = new PublicPropertiesAndFieldsAnalyzer();
            _defaultWriterInstructors = new List<IJsonWriterInstructor>().AddDefaultWriterInstructors(primitiveTypeToFormattersMapping,
                                                                                                      _readableValuesTypeAnalyzer);

            _writerFactory = new JsonWriterNormalizedKeysDecoratorFactory();

            _instructorCache = new Dictionary<Type, IJsonWriterInstructor>();
        }

        public Func<IList<IJsonWriterInstructor>> CreateWriterInstructorList
        {
            get { return _createWriterInstructorList; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _createWriterInstructorList = value;
            }
        }

        private static IList<IJsonWriterInstructor> CreateList()
        {
            return new List<IJsonWriterInstructor>();
        }

        public JsonSerializerBuilder WithWriterInstructors(IList<IJsonWriterInstructor> writerInstructors)
        {
            _defaultWriterInstructors = writerInstructors;
            return this;
        }

        public JsonSerializerBuilder WithWriterFactory(IJsonWriterFactory writerFactory)
        {
            _writerFactory = writerFactory;
            return this;
        }

        public JsonSerializerBuilder WithInstructorCache(IDictionary<Type, IJsonWriterInstructor> instructorCache)
        {
            _instructorCache = instructorCache;
            return this;
        }

        public JsonSerializerBuilder WithTypeAnalyzerForRules(IReadableValuesTypeAnalyzer typeAnalyzer)
        {
            _readableValuesTypeAnalyzer = typeAnalyzer;
            return this;
        }

        public JsonSerializerBuilder WithRuleFor<T>(Action<Rule<T>> configureRule)
        {
            var targetType = typeof (T);
            var targetRule = (Rule<T>) _rules.FirstOrDefault(r => r.TargetType == targetType);
            if (targetRule == null)
            {
                targetRule = new Rule<T>(_readableValuesTypeAnalyzer);
                _rules.Add(targetRule);
            }

            configureRule(targetRule);
            return this;
        }

        public ISerializer Build()
        {
            var writerInstructors = CreateWriterInstructorList();
            foreach (var defaultWriterInstructor in _defaultWriterInstructors)
            {
                writerInstructors.Add(defaultWriterInstructor);
            }

            for (var i = 0; i < _rules.Count; i++)
            {
                var targetRule = _rules[i];
                var customInstructor = targetRule.CreateInstructor();
                writerInstructors.Insert(i, customInstructor);
                if (_instructorCache.ContainsKey(targetRule.TargetType) == false)
                    _instructorCache.Add(targetRule.TargetType, customInstructor);
            }

            return new JsonSerializer(writerInstructors, _writerFactory, _instructorCache);
        }
    }
}