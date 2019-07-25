namespace SimpleDocumentStore
{
    public interface ISimpleDocumentBase<T> : IDocumentBaseWithTenant
    {
        MetaData MetaData { get; }
        T Document { get; }

    }
}
