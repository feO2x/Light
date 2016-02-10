using System;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Serialization.Json
{
    public sealed class JsonSerializer : ISerializer
    {
        private readonly IList<IJsonWriterInstructor> _writerInstructors;
        private readonly IJsonWriterFactory _writerFactory;
        private readonly IDictionary<Type, IJsonWriterInstructor> _instructorCache;
        private IJsonWriter _jsonWriter;

        public JsonSerializer(IList<IJsonWriterInstructor> writerInstructors,
                              IJsonWriterFactory writerFactory,
                              IDictionary<Type, IJsonWriterInstructor> instructorCache)
        {
            writerInstructors.MustNotBeNull(nameof(writerInstructors));
            writerFactory.MustNotBeNull(nameof(writerFactory));
            instructorCache.MustNotBeNull(nameof(instructorCache));

            _writerInstructors = writerInstructors;
            _writerFactory = writerFactory;
            _instructorCache = instructorCache;
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
            SerializeObject(objectGraphRoot, objectGraphRoot.GetType(), referencedType);

            var json = _writerFactory.FinishWriteProcessAndReleaseResources();
            _jsonWriter = null;
            return json;
        }

        private void SerializeObject(object @object, Type actualType, Type referencedType)
        {
            IJsonWriterInstructor targetWriterInstructor;
            if (_instructorCache.TryGetValue(actualType, out targetWriterInstructor) == false)
            {
                targetWriterInstructor = FindTargetInstructor(@object, actualType, referencedType);
                if (targetWriterInstructor == null)
                    throw new SerializationException($"Type {actualType.FullName} cannot be serialized because there is no IJsonWriterInstructor registered that can cover this type.");

                _instructorCache.Add(actualType, targetWriterInstructor);
            }

            targetWriterInstructor.Serialize(new JsonSerializationContext(@object, actualType, referencedType, SerializeObject, _jsonWriter));
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
