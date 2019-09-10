using AzureTableStorageRepository;
using AzureTableStorageRepository.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseSetup
{
    class Program
    {
        static AppSettings _settings;
        const string defaultPartitionKey = "Default";
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _settings = config.GetSection("AppSettings").Get<AppSettings>();
            InitCarBookingData().GetAwaiter().GetResult();
            InitCars().GetAwaiter().GetResult();
            Console.WriteLine("Hello World!");
        }


        static async Task InitCarBookingData()
        {
            try
            {
                // Init Supplier
                var repo = new AzureTableStorage<Supplier>(
                       new AzureTableSettings(
                           storageAccount: _settings.StorageAccount,
                           storageKey: _settings.StorageKey,
                           tableName: "Suppliers"));
                var partitionKey = defaultPartitionKey;
                await repo.Insert(new Supplier { Id = 1, Name = SupplierEnum.Sixt.ToString(), RowKey = "1", PartitionKey = partitionKey });
                await repo.Insert(new Supplier { Id = 2, Name = SupplierEnum.Goldcar.ToString(), RowKey = "2", PartitionKey = partitionKey });
                await repo.Insert(new Supplier { Id = 3, Name = SupplierEnum.Budget.ToString(), RowKey = "3", PartitionKey = partitionKey });
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message}");
            }
            

            
        }
        //const string defaultSupplierPartitionKey = "All-Suppliers";
        //const string defaultCarPartitionKey = "All-Cars";
        static async Task InitCars()
        {
            try
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var dataFile = Path.Combine(dir,"data" ,"cars.json");
                var dataString = File.ReadAllTextAsync(dataFile).GetAwaiter().GetResult();
                var results = JsonConvert.DeserializeObject<PriceResultWrapper>(dataString);
                var incrementId = 0;
                var partitionKey = defaultPartitionKey;
                var cars = new List<Car>();
                var items = results.PriceResults.Take(100);
                foreach (var r in items)
                {
                    incrementId++;
                    var car = new Car
                    {
                        Id = incrementId,
                        Name = r.CarName,
                        ImageUrl = r.CarImage,
                        CarClassId = new Random().Next(1, 3),
                        Doors = r.Doors,
                        Seats = r.Seats,
                        RowKey = incrementId.ToString(),
                        PartitionKey = partitionKey
                    };

                    cars.Add(car);
                }
                var repo = new AzureTableStorage<Car>(
                      new AzureTableSettings(
                          storageAccount: _settings.StorageAccount,
                          storageKey: _settings.StorageKey,
                          tableName: "Cars"));
                await repo.InsertManyAsync(cars.ToArray());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message}");
            }
          
        }

    }

    public class PriceResultWrapper
    {
        public List<PriceResult> PriceResults { get; set; }
    }
    public class PriceResult
    {
        public int SupplierId { get; set; }
        public decimal LocalPrice { get; set; }
        public string CarName { get; set; }
        public string CarImage { get; set; }
        public string CarClassId { get; set; }
        public string Doors { get; set; }
        public string Seats { get; set; }

    }
}
