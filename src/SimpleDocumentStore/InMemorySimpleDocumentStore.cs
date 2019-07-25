using System;

namespace SimpleDocumentStore
{
    public class InMemorySimpleDocumentStore<T> : 
        InMemoryStoreBase<SimpleDocument<T>>,
        ISimpleDocumentStore<T> where T : class, IComparable, IDocumentBase, new()
    {
    }
}