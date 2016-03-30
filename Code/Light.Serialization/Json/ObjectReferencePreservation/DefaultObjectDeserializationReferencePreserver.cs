using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.GuardClauses;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public class DefaultObjectDeserializationReferencePreserver : IObjectDeserializationReferencePreserver
    {
        private readonly IDictionary<int, object> _references;

        public DefaultObjectDeserializationReferencePreserver(IDictionary<int, object> references)
        {
            references.MustNotBeNull(nameof(references));

            _references = references;
        }

        public void AddReference(int id, object @object)
        {
            id.MustNotBeLessThan(0);
            @object.MustNotBeNull(nameof(@object));

            if (_references.ContainsKey(id))
                return;

            _references.Add(id, @object);
        }

        public bool TryGetReference(int id, out object @object)
        {
            id.MustNotBeLessThan(0);

            return _references.TryGetValue(id, out @object);
        }
    }
}
