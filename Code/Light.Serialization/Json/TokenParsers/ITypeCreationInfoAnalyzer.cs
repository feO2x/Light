using System;

namespace Light.Serialization.Json.TokenParsers
{
    public interface ITypeCreationInfoAnalyzer
    {
        TypeConstructionInfo CreateInfo(Type typeToAnalyze);
    }
}