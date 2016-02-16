using System;
using Light.Serialization.Json.ComplexTypeConstruction;

namespace Light.Serialization.Json.TokenParsers
{
    public interface ITypeCreationInfoAnalyzer
    {
        TypeCreationDescription CreateInfo(Type typeToAnalyze);
    }
}