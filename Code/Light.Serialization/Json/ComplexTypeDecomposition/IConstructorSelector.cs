using System;
using System.Reflection;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public interface IConstructorSelector
    {
        ConstructorInfo SelectConstructor(ConstructorInfo[] constructorInfos, Type typeToAnalyze);
    }
}