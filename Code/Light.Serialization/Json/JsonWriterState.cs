namespace Light.Serialization.Json
{
    public enum JsonWriterState
    {
        AtDocumentBeginning,
        StartedComplexObject,
        FinishedComplexObject,
        StartedArray,
        FinishedArray,
        WroteValue,
        WroteKey,
        WroteDelimiter,
        AtDocumentEnd
    }
}