using AzureTableStorageRepository;
using AzureTableStorageRepository.Models;
using System;
using System.Threading.Tasks;

namespace DurableFunctionPresentation.Services
{
    public class CustomerDataService
    {
        ICustomerRepository _customerRepository;
        public CustomerDataService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Guid> AddCustomer(Customer model)
        {
            return await _customerRepository.AddCustomer(model);
        }

        public async Task<Customer> GetCustomer(string username)
        {
            return await _customerRepository.GetCustomer(username);
        }
    }
}
