using CustomerManagementStore.Serivices;
using CustomerManagementStore.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CustomerManagementStore.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddInMemoryCustomerManagmentStore(this IServiceCollection services)
        {

            services.TryAddSingleton<ICustomerManagmentStore,InMemoryCustomerManagmentStore> ();
        }
    }
}
