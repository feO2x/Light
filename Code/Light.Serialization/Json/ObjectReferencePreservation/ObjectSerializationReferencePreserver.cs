using System;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public sealed class ObjectSerializationReferencePreserver
    {
        private readonly Dictionary<object, uint> _serializedReferences;
        private uint _currentId;

        public ObjectSerializationReferencePreserver(Dictionary<object, uint> serializedReferences)
        {
            _serializedReferences = serializedReferences;
        }

        public JsonReferenceInfo GetObjectReferenceInfo(object @object)
        {
            @object.MustNotBeNull(nameof(@object));

            uint jsonObjectId;

            if (_serializedReferences.TryGetValue(@object, out jsonObjectId))
            {
                return new JsonReferenceInfo(true, jsonObjectId);
            }

            return new JsonReferenceInfo(false, GetNewId());
        }

        private uint GetNewId()
        {
            return _currentId++;
        }
    }
}