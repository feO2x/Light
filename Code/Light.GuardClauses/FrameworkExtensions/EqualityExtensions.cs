namespace Light.GuardClauses.FrameworkExtensions
{
    public static class EqualityExtensions
    {
        public static bool EqualsWithHashCode<T>(this T value, T other)
        {
            if (value == null)
                return other == null;

            if (other == null)
                return false;

            var hashCodeCompareResult = value.GetHashCode() == other.GetHashCode();
            return hashCodeCompareResult && value.Equals(other);
        }
    }
}
