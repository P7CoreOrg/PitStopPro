using CustomerManagementStore.Model;
using CustomerManagementStore.Serivices;
using SimpleDocumentStore;
using System;
using System.Threading.Tasks;

namespace CustomerManagementStore.Stores
{
    public class InMemoryCustomerManagmentStore : InMemorySimpleDocumentStore<Customer>,ICustomerManagmentStore
    {
        public async Task<Customer> GetCustomerAsync(string id)
        {
            var result = await base.FetchAsync(id);
            return result?.Document;
        }
        public async Task UpsertCustomerAsync(Customer customer)
        {
            await base.InsertAsync(new SimpleDocument<Customer>(new MetaData()
            {
                Category = "Category 1", Version = "1.0.0"
            }, customer)
            {
                Id = customer.Id
            });
        }
        public async Task RemoveCustomerAsync(string id)
        {
            await base.DeleteAsync(id);
        }
    }
}
