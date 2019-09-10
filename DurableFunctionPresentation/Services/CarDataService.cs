using AzureTableStorageRepository;
using AzureTableStorageRepository.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunctionPresentation.Services
{
    //public interface ICarService
    //{

    //}

    public class CarDataService
    {
        ICarRepository _carRepostory;
        public CarDataService(ICarRepository carRepo)
        {
            _carRepostory = carRepo;
        }
        public async Task<List<Supplier>> GetActiveSuppliers()
        {
            return await _carRepostory.GetActiveSuppliersAsync();
        }

       
    }
}
