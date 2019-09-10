using AzureTableStorageRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureTableStorageRepository
{
    public class CarAzureTableRepository : ICarRepository
    {
        AzureTableStorage<Car> _carTable;
        AzureTableStorage<Supplier> _supplierTable;
       
        //const string defaultPartitionKey = "Default";

        /// <summary>
        /// TODO: split service and repository based on domain like Car, Supplier, Booking, etc.
        /// </summary>
        /// <param name="carAzureTable"></param>
        /// <param name="supplierAzureTable"></param>
        public CarAzureTableRepository(AzureTableStorage<Car> carAzureTable, AzureTableStorage<Supplier> supplierAzureTable)
        {
            _carTable = carAzureTable;
            _supplierTable = supplierAzureTable;
        }

        public async Task<List<Supplier>> GetActiveSuppliersAsync()
        {
            // TODO: filter active status by forming query to Azure Table, for now just ingore the filter
            return await _supplierTable.GetList(Constants.DefaultPartitionKey);
        }

        public async Task<List<Car>> GetActiveCarsAsync()
        {
            // TODO: filter active status by forming query to Azure Table, for now just ingore the filter
            return await _carTable.GetList(Constants.DefaultPartitionKey);
        }

        
    }
}
