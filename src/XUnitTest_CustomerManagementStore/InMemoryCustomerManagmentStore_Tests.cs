using System;
using CustomerManagementStore.Model;
using CustomerManagementStore.Stores;
using FluentAssertions;
using Xunit;

namespace XUnitTest_CustomerManagementStore
{
    public class InMemoryCustomerManagmentStore_Tests
    {
        private static string GuidS => Guid.NewGuid().ToString("N");
        Customer CreateRandomCustomer()
        {
            var id = GuidS;
            return new Customer()
            {
                Id = id,
                Address = GuidS,
                City = GuidS,
                EmailAddress = $"{id}@donkey.com",
                Name = $"{id} donkey",
                PostalCode = GuidS,
                TelephoneNumber = GuidS
            };
        }
        [Fact]
        public void success_ctor()
        {
            var store = new InMemoryCustomerManagmentStore();
            store.Should().NotBeNull();

        }
        [Fact]
        public async void success_null_customer()
        {
            var store = new InMemoryCustomerManagmentStore();
            var c = await store.GetCustomerAsync(GuidS);
            c.Should().BeNull();
        }
        [Fact]
        public async void success_delete_nonexistant_customer()
        {
            var store = new InMemoryCustomerManagmentStore();
            await store.RemoveCustomerAsync(GuidS);
        }

        [Fact]
        public async void success_upsert()
        {
            var store = new InMemoryCustomerManagmentStore();
            var customer = CreateRandomCustomer();

            await store.UpsertCustomerAsync(customer);
            var c = await store.GetCustomerAsync(customer.Id);
            c.Should().NotBeNull();
            c.Should().BeSameAs(customer);
        }
        [Fact]
        public async void success_upsert_and_remove()
        {
            var store = new InMemoryCustomerManagmentStore();
            var customer = CreateRandomCustomer();

            await store.UpsertCustomerAsync(customer);
            var c = await store.GetCustomerAsync(customer.Id);
            c.Should().NotBeNull();
            c.Should().BeSameAs(customer);

            await store.RemoveCustomerAsync(c.Id);
            c = await store.GetCustomerAsync(customer.Id);
            c.Should().BeNull();
        }
   
    }
}
