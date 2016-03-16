using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.LowLevelWriting;
using Light.Serialization.Json.ObjectReferencePreservation;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.SerializationRules;

namespace Light.Serialization.Json
{
    public class JsonSerializerBuilder
    {
        private readonly List<Rule> _rules = new List<Rule>();
        private Func<IList<IJsonWriterInstructor>> _createWriterInstructorList = CreateList;
        private IList<IJsonWriterInstructor> _defaultWriterInstructors;
        private IDictionary<Type, IJsonWriterInstructor> _instructorCache;
        private IReadableValuesTypeAnalyzer _readableValuesTypeAnalyzer;
        private IJsonWriterFactory _writerFactory;
        private IRuleBuilder _ruleBuilder;

        public JsonSerializerBuilder()
        {
            var characterEscaper = new DefaultCharacterEscaper();
            var primitiveTypeToFormattersMapping = new List<IPrimitiveTypeFormatter>().AddDefaultPrimitiveTypeFormatters(characterEscaper)
                                                                                      .ToDictionary(f => f.TargetType);

            UseDefaultTypeAnalyzer();
            _defaultWriterInstructors = new List<IJsonWriterInstructor>().AddDefaultWriterInstructors(primitiveTypeToFormattersMapping,
                                                                                                      _readableValuesTypeAnalyzer);

            UseDefaultWriterFactory();
            _instructorCache = new Dictionary<Type, IJsonWriterInstructor>();
            UseDefaultRuleBuilder();
        }

        public JsonSerializerBuilder WithCreateFunctionForInstructorList(Func<IList<IJsonWriterInstructor>> createList)
        {
            createList.MustNotBeNull(nameof(createList));

            _createWriterInstructorList = createList;
            return this;
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

        public JsonSerializerBuilder WithTypeAnalyzer(IReadableValuesTypeAnalyzer typeAnalyzer)
        {
            _readableValuesTypeAnalyzer = typeAnalyzer;
            return this;
        }

        private void UseDefaultRuleBuilder()
        {
            _ruleBuilder = new PreservedObjectsRuleBuilder();
        }

        public JsonSerializerBuilder UseDefaultTypeAnalyzer()
        {
            return UseDefaultTypeAnalyzer(new Dictionary<Type, IList<IValueProvider>>());
        }

        public JsonSerializerBuilder UseDefaultTypeAnalyzer(IDictionary<Type, IList<IValueProvider>> valueProvidersCache)
        {
            _readableValuesTypeAnalyzer = new ValueProvidersCacheDecorator(new PublicPropertiesAndFieldsAnalyzer(),
                                                                           valueProvidersCache);
            return this;
        }

        public JsonSerializerBuilder UseDefaultWriterFactory()
        {
            _writerFactory = new JsonWriterFactory();
            return this;
        }

        public JsonSerializerBuilder WithRuleBuilder(IRuleBuilder builder)
        {
            _ruleBuilder = builder;
            return this;
        }

        public JsonSerializerBuilder WithRuleFor<T>(Action<Rule<T>> configureRule)
        {
            var targetType = typeof (T);
            var targetRule = (Rule<T>) _rules.FirstOrDefault(r => r.TargetType == targetType);
            if (targetRule == null)
            {
                targetRule = _ruleBuilder.CreateRule<T>(_readableValuesTypeAnalyzer);
                _rules.Add(targetRule);
            }

            configureRule(targetRule);
            return this;
        }

        public ISerializer Build()
        {
            var writerInstructors = _createWriterInstructorList();
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

            return new JsonSerializer((IReadOnlyList<IJsonWriterInstructor>) writerInstructors, _writerFactory, _instructorCache);
        }
    }
}