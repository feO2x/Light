using System;
using System.Collections.Generic;
using System.Linq;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.Writing;

namespace Light.Serialization.Json
{
    public class JsonSerializerBuilder
    {
        private IJsonWriterFactory _writerFactory;
        private IList<IJsonWriterInstructor> _writerInstructors;
        private IDictionary<Type, IJsonWriterInstructor> _instructorCache; 

        public JsonSerializerBuilder()
        {
            var characterEscaper = new DefaultCharacterEscaper();
            var primitiveTypeToFormattersMapping = new List<IPrimitiveTypeFormatter>().AddDefaultPrimitiveTypeFormatters(characterEscaper)
                                                                                      .ToDictionary(f => f.TargetType);

            var publicPropertiesAndFieldsAnalyzer = new PublicPropertiesAndFieldsAnalyzer();
            _writerInstructors = new List<IJsonWriterInstructor>().AddDefaultWriterInstructors(primitiveTypeToFormattersMapping,
                                                                                               publicPropertiesAndFieldsAnalyzer);

            _writerFactory = new JsonWriterFactory();

            _instructorCache = new Dictionary<Type, IJsonWriterInstructor>();
        }

        public JsonSerializerBuilder WithWriterInstructors(IList<IJsonWriterInstructor> writerInstructors)
        {
            _writerInstructors = writerInstructors;
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

        public ISerializer Build()
        {
            return new JsonSerializer(_writerInstructors, _writerFactory, _instructorCache);
        }
    }
}