namespace Light.Serialization.Json
{
    public enum JsonTokenType
    {
        String,
        IntegerNumber,
        FloatingPointNumber,
        BeginOfObject,
        BeginOfArray,
        EndOfObject,
        EndOfArray,
        ValueDelimiter,
        PairDelimiter,
        True,
        False,
        Null,
        EndOfDocument,
        Error
    }
}