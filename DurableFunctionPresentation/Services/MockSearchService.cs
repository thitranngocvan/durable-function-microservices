using AzureTableStorageRepository.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunctionPresentation.Services
{
    public static class MockSearchService
    {
        static PriceResultWrapper _searchResultFromDataFile;
        public static bool IsLocalHost { get; set; }
        static MockSearchService()
        {
            
           
        }

        public static void InitData()
        {
            var dir = IsLocalHost ? Environment.CurrentDirectory : "d://home//site//wwwroot";
            var dataFile = Path.Combine(dir, "data", "cars.json");
            var dataString = File.ReadAllTextAsync(dataFile).GetAwaiter().GetResult();
            _searchResultFromDataFile = JsonConvert.DeserializeObject<PriceResultWrapper>(dataString);
        }

        public static async Task<List<SupplierCarPrice>> SearchCars(CarSearchRequest searchRequest, int supplierId)
        {
            var cars = _searchResultFromDataFile.PriceResults.Where(c => c.SupplierId == supplierId).Take(50);
            return cars.Select(c => new SupplierCarPrice {
                Currency = searchRequest.CurrencyCode,
                SupplierBasePrice = c.SupplierPrice,
                SupplierCar = new SupplierCar {
                    Name = c.CarName,
                    Seats = c.Seats,
                    ImageUrl = c.CarImage
                }
                

            }).ToList();
           
        }


    }

   
}
