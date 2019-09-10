using AzureTableStorageRepository.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class MockDataProvider
{
    public class PriceResult
    {
        public int SupplierId { get; set; }
        public decimal SupplierPrice { get; set; }
        public string CarName { get; set; }
        public string CarImage { get; set; }
        public string CarClassId { get; set; }
        public string Doors { get; set; }
        public string Seats { get; set; }
      
    }
    public async Task<List<SupplierCarPrice>> SearchCars(int supplierId)
    {
        var dataFile = Path.Combine(Environment.CurrentDirectory, "Data", "cars.json");
        var dataString = await File.ReadAllTextAsync(dataFile);
        var results = JsonConvert.DeserializeObject<List<PriceResult>>(dataString);
        if(results != null && results.Any())
        {
            return results.Where(p => p.SupplierId == supplierId).Select(p => new SupplierCarPrice
            {
                Currency = CurrencyEnum.GBP.ToString(),
                SupplierBasePrice = p.SupplierPrice,
                SupplierCar = new SupplierCar
                {
                    Name = p.CarName,
                    CarClassId = p.CarClassId,
                    Doors = p.Doors,
                    Seats = p.Seats
                }
            }).ToList();
        }

        return null;
    }
}
