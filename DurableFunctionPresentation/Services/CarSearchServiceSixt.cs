using AzureTableStorageRepository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctionPresentation.Services
{
    public class CarSearchServiceSixt : ISupplierCarSearchService
    {
        public async Task<List<SupplierCarPrice>> SearchCars(CarSearchRequest searchRequest)
        {
            return await MockSearchService.SearchCars(searchRequest, (int)SupplierEnum.Sixt);
        }
    }
}
