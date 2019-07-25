namespace SimpleDocumentStore
{
    public interface ISimpleDocumentBase<T> : IDocumentBase
    {
        MetaData MetaData { get; }
        T Document { get; }

    }
}
