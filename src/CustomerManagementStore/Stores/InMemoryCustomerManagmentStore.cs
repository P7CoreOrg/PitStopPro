using CustomerManagementStore.Model;
using CustomerManagementStore.Serivices;
using SimpleDocumentStore;
using System;
using System.Threading.Tasks;

namespace CustomerManagementStore.Stores
{
    public class InMemoryCustomerManagmentStore : 
        SimpleDocument<Customer>,ICustomerManagmentStore
    {
        public Task<Customer> GetCustomerAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
