using CustomerManagementStore.Model;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementStore.Serivices
{
    public interface ICustomerManagmentStore
    {
        Task<Customer> GetCustomerAsync(string id);
    }
}
