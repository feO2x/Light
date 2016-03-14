namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public struct JsonReferenceInfo
    {
        public readonly bool WasAlreadySerialized;
        public readonly uint JsonObjectId;

        public JsonReferenceInfo(bool wasAlreadySerialized, uint jsonObjectId)
        {
            WasAlreadySerialized = wasAlreadySerialized;
            JsonObjectId = jsonObjectId;
        }
    }
}