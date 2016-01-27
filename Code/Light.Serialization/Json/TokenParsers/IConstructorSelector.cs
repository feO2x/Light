using System;
using System.Reflection;

namespace Light.Serialization.Json.TokenParsers
{
    public interface IConstructorSelector
    {
        ConstructorInfo SelectConstructor(ConstructorInfo[] constructorInfos, Type typeToAnalyze);
    }
}