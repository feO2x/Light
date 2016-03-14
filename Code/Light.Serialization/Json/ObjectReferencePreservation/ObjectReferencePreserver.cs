using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public sealed class ObjectReferencePreserver
    {
        private readonly Dictionary<object, int> _serializedReferences;
        private uint _currentId;

        public ObjectReferencePreserver(Dictionary<object, int> serializedReferences)
        {
            _serializedReferences = serializedReferences;
        }

        public JsonReferenceInfo GetObjectReferenceInfo(object @object)
        {
            if (_serializedReferences.ContainsKey(@object))
                throw new NotImplementedException();

            throw new NotImplementedException();
        }

        private uint GetNewId()
        {
            return _currentId++;
        }
    }
}