
namespace Light.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T objectGraphRoot);
    }
}
