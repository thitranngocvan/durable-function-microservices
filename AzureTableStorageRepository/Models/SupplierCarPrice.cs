namespace AzureTableStorageRepository.Models
{
    public class SupplierCarPrice
    {
        public SupplierCar SupplierCar { get; set; }

        public decimal SupplierBasePrice { get; set; }

        public string Currency { get; set; }
    }
}
