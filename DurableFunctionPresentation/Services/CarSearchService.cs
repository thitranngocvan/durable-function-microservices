using AzureTableStorageRepository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctionPresentation.Services
{
    public class CarSearchService
    {
        public async Task<List<CarPrice>> SearchCars(CarSearchRequest searchRequest, Supplier supplier)
        {
            var factory = new SupplierCarSearchFactory();
            var supplierSearchService = factory.GetSupplierCarSearchService(supplier.Id);
            var supplierCarPrice =  await supplierSearchService.SearchCars(searchRequest);

            var mappedResult = new List<CarPrice>();

            foreach (var sc in supplierCarPrice)
            {
                // do some mapping here to identify the car between supplier car and local car
                // Apply commission policy here, now just multiple 10% as an example
                var carPrice = new CarPrice()
                {
                    Car = new Car
                    {
                        Id = new Random().Next(1, 100),
                        Name = sc.SupplierCar.Name
                    },
                    Price = sc.SupplierBasePrice * 0.1M
                    //Supplier = supplier,
                    //BookingDetail = new BookingRequestDetail {
                    //    PickupDate = searchRequest.PickupDate,
                    //    ReturnDate = searchRequest.ReturnDate,
                    //    PickupLocation = new Location { Name = searchRequest.PickupLocation },
                    //    ReturnLocation = new Location { Name = searchRequest.ReturnLocation }
                    //}
                };

                mappedResult.Add(carPrice);
            }
            return mappedResult;
        }
    }
}
