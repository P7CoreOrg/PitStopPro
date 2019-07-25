using System;

namespace SimpleDocumentStore
{
    public interface IDocumentBaseWithTenant : IDocumentBase
    {
        Guid TenantId_G { get; }
        string TenantId { get; }
    }
}
