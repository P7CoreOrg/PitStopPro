using System;

namespace SimpleDocumentStore
{
    public interface IDocumentBase
    {
        Guid Id_G { get; }
        string Id { get; }
    }
}
