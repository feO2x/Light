using System;
using Light.GuardClauses;

namespace Light.BayesianNetwork.FrameworkExtensions
{
    public abstract class GuidEntity : IEntity<Guid>, IEquatable<IEntity<Guid>>
    {
        private readonly Guid _id;
        private readonly int _hashCode;

        public Guid Id => _id;

        protected GuidEntity(Guid id)
        {
            id.MustNotBeEmpty(nameof(id));

            _id = id;
            _hashCode = id.GetHashCode();
        }


        public bool Equals(IEntity<Guid> other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return other.Id == _id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IEntity<Guid>);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public static bool operator ==(GuidEntity first, GuidEntity second)
        {
            return ReferenceEquals(first, null) ? ReferenceEquals(second, null) : first.Equals(second);
        }

        public static bool operator !=(GuidEntity first, GuidEntity second)
        {
            return !(first == second);
        }
    }
}