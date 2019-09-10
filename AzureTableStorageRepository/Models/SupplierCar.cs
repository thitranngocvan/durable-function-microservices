namespace AzureTableStorageRepository.Models
{
    public class SupplierCar
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CarClassId { get; set; }
        public string Doors { get; set; }
        public string Seats { get; set; }

        // public Supplier Supplier { get; set; }
    }
}
