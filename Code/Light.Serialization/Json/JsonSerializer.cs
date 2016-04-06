using System;
using System.Collections;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.WriterInstructors;

namespace Light.Serialization.Json
{
    public sealed class JsonSerializer : ISerializer
    {
        private readonly IReadOnlyList<IJsonWriterInstructor> _writerInstructors;
        private readonly IJsonWriterFactory _writerFactory;
        private readonly IDictionary<Type, IJsonWriterInstructor> _instructorCache;
        private IJsonWriter _jsonWriter;
        private int _currentIndentLevel;
        private readonly int _maxIndentLevel;

        public JsonSerializer(IReadOnlyList<IJsonWriterInstructor> writerInstructors,
                              IJsonWriterFactory writerFactory,
                              IDictionary<Type, IJsonWriterInstructor> instructorCache,
                              int maxIndentLevel = 20)
        {
            writerInstructors.MustNotBeNull(nameof(writerInstructors));
            writerFactory.MustNotBeNull(nameof(writerFactory));
            instructorCache.MustNotBeNull(nameof(instructorCache));
            maxIndentLevel.MustNotBeLessThan(1);

            _writerInstructors = writerInstructors;
            _writerFactory = writerFactory;
            _instructorCache = instructorCache;
            _maxIndentLevel = maxIndentLevel;
        }

        public string Serialize<T>(T objectGraphRoot)
        {
            return Serialize(objectGraphRoot, typeof (T));
        }

        public string Serialize(object objectGraphRoot, Type referencedType)
        {
            objectGraphRoot.MustNotBeNull(nameof(objectGraphRoot));
            referencedType.MustNotBeNull(nameof(referencedType));

            _jsonWriter = _writerFactory.Create();
            SerializeObject(objectGraphRoot, objectGraphRoot.GetType(), referencedType, true);

            var json = _writerFactory.FinishWriteProcessAndReleaseResources();
            _jsonWriter = null;
            return json;
        }

        private void SerializeObject(object @object, Type actualType, Type referencedType, bool increaseIndent)
        {
            IJsonWriterInstructor targetWriterInstructor;
            if (_instructorCache.TryGetValue(actualType, out targetWriterInstructor) == false)
            {
                targetWriterInstructor = FindTargetInstructor(@object, actualType, referencedType);
                if (targetWriterInstructor == null)
                    throw new SerializationException($"Type {actualType.FullName} cannot be serialized because there is no IJsonWriterInstructor registered that can cover this type.");

                _instructorCache.Add(actualType, targetWriterInstructor);
            }

            if (increaseIndent)
                _currentIndentLevel++;

            if(_currentIndentLevel > _maxIndentLevel)
                throw new SerializationException($"Serializing {@object} would produce the indent of {_currentIndentLevel} which exceeds the maximal indent of {_maxIndentLevel}.");

            var decreaseIndent = targetWriterInstructor.Serialize(new JsonSerializationContext(@object, actualType, referencedType, SerializeObject, _jsonWriter));

            if (decreaseIndent)
                _currentIndentLevel--;
        }

        private IJsonWriterInstructor FindTargetInstructor(object @object, Type objectType, Type referencedType)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var writerInstructor in _writerInstructors)
            {
                if (writerInstructor.AppliesToObject(@object, objectType, referencedType))
                    return writerInstructor;
            }
            return null;
        }
    }
}
