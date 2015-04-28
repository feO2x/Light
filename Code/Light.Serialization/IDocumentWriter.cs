
namespace Light.Serialization
{
    public interface IDocumentWriter
    {
        void BeginDocument();
        void EndDocument();
        string Document { get; }
    }
}
