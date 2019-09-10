using AzureTableStorageRepository.Models;
using System;
using System.Threading.Tasks;

namespace AzureTableStorageRepository
{
    public interface ICustomerRepository
    {
        Task<Guid> AddCustomer(Customer model);

        Task<Customer> GetCustomer(string username);
    }
}
