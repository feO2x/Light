using System;

namespace Light.BayesianNetwork.FrameworkExtensions
{
    public interface IEntity<out T> where T : IEquatable<T>
    {
        T Id { get; }
    }
}