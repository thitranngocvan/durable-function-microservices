using AzureTableStorageRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctionPresentation.Services
{
    public interface ISupplierCarSearchService
    {
        Task<List<SupplierCarPrice>> SearchCars(CarSearchRequest searchRequest);
    }
}
