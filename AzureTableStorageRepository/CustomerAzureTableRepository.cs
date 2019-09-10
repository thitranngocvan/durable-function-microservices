using AzureTableStorageRepository.Models;
using System;
using System.Threading.Tasks;

namespace AzureTableStorageRepository
{
    public class CustomerAzureTableRepository : ICustomerRepository
    {
        AzureTableStorage<Customer> _customerStorage;
        public CustomerAzureTableRepository(AzureTableStorage<Customer> customerStorage)
        {
            _customerStorage = customerStorage;
        }
        public async Task<Guid> AddCustomer(Customer model)
        {
            model.CustomerGuid = Guid.NewGuid();
            model.RowKey = model.Username;
            model.PartitionKey = Constants.DefaultPartitionKey;
            await _customerStorage.Insert(model);
            return model.CustomerGuid;
        }

        public async Task<Customer> GetCustomer(string username)
        {
            return await _customerStorage.GetItem(Constants.DefaultPartitionKey, username);
        }
    }
}
