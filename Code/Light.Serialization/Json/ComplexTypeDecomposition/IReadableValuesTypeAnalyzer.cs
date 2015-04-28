using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public interface IReadableValuesTypeAnalyzer
    {
        IList<IValueProvider> AnalyzeType(Type type);
    }
}