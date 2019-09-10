using AzureTableStorageRepository.Models;
using System;

namespace DurableFunctionPresentation.Services
{
    public class SupplierCarSearchFactory
    {
        public ISupplierCarSearchService GetSupplierCarSearchService(int supplierId) {
            switch (supplierId)
            {
                case (int)SupplierEnum.Sixt: return new CarSearchServiceSixt();
                case (int)SupplierEnum.Goldcar: return new CarSearchServiceGoldcar();
                case (int)SupplierEnum.Budget: return new CarSearchServiceBudget();
                default: throw new InvalidOperationException($"Supplier {supplierId} is not supported!");
            };
        }
    }
}
