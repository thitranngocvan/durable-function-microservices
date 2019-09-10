using AzureTableStorageRepository;
using AzureTableStorageRepository.Models;
using DurableFunctionPresentation.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DurableFunctionPresentation
{
    public static class ServiceProvider
    {
        public static CarDataService GetCarService()
        {
            var carAzureTable = new AzureTableStorage<Car>(Settings.CarTableSetting);
            var supplierAzureTable = new AzureTableStorage<Supplier>(Settings.SupplierTableSetting);
            //var customerAzureTable = new AzureTableStorage<Customer>(Settings.CustomerTableSetting);
            //var bookingAzureTable = new AzureTableStorage<Booking>(Settings.BookingTableSetting);
            //var paymentAzureTable = new AzureTableStorage<Payment>(Settings.PaymentTableSetting);
            //var carPriceAzureTable = new AzureTableStorage<BookingCarPrice>(Settings.CarPriceTableSetting);

            var carRepository = new CarAzureTableRepository(carAzureTable, supplierAzureTable);
            //var carAzure
            return new CarDataService(carRepository);
        }

        public static CustomerDataService GetCustomerDataService()
        {
            var customerAzureTable = new AzureTableStorage<Customer>(Settings.CustomerTableSetting);
            var customerRepository = new CustomerAzureTableRepository(customerAzureTable);
            return new CustomerDataService(customerRepository);
        }

        public static CarBookingService GetCarBookingService()
        {
            var bookingAzureTable = new AzureTableStorage<Booking>(Settings.BookingTableSetting);
            //AzureTableStorage<BookingCarPrice>
            var bookingCarPriceTable = new AzureTableStorage<BookingCarPrice>(Settings.BookingCarPriceTableSetting);
            var bookingRepository = new BookingAzureTableRepositor(bookingAzureTable, bookingCarPriceTable);

            return new CarBookingService(bookingRepository);
        }
    }
}
