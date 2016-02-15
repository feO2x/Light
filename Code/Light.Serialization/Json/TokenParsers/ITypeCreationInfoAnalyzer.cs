using System;

namespace Light.Serialization.Json.TokenParsers
{
    public interface ITypeCreationInfoAnalyzer
    {
        TypeCreationInfo CreateInfo(Type typeToAnalyze);
    }
}