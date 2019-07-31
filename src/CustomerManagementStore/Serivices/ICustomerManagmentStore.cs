using CustomerManagementStore.Model;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementStore.Serivices
{
    public interface ICustomerManagmentStore
    {
        Task<Customer> GetCustomerAsync(string id);
        Task<Customer> UpsertCustomerAsync(Customer customer);
        Task RemoveCustomerAsync(string id);
    }
}
