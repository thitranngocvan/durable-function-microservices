using AzureTableStorageRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DurableFunctionPresentation
{
    public static class Settings
    {
        public static string StorageAccount = Environment.GetEnvironmentVariable("StorageAccount");

        public static string StorageKey = Environment.GetEnvironmentVariable("StorageKey");

        public static AzureTableSettings CarTableSetting = new AzureTableSettings(StorageAccount, StorageKey, "Cars");
        public static AzureTableSettings SupplierTableSetting = new AzureTableSettings(StorageAccount, StorageKey, "Suppliers");

        public static AzureTableSettings CustomerTableSetting = new AzureTableSettings(StorageAccount, StorageKey, "Customers");

        public static AzureTableSettings BookingTableSetting = new AzureTableSettings(StorageAccount, StorageKey, "Bookings");

        public static AzureTableSettings BookingCarPriceTableSetting = new AzureTableSettings(StorageAccount, StorageKey, "BookingCarPrices");

        public static AzureTableSettings PaymentTableSetting = new AzureTableSettings(StorageAccount, StorageKey, "Payments");

        public static string SendGridKey = Environment.GetEnvironmentVariable("SendGridKey");

        public static string BaseUrl = Environment.GetEnvironmentVariable("BaseUrl");


    }
}
