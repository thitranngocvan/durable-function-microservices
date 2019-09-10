using System;

namespace AzureTableStorageRepository.Models
{
    public class BookingCarPrice : BaseModel
    {
        public Guid BookingCarPriceGuid { get; set; }
        public int CarId { get; set; }

        public string CarName { get; set; }

        public decimal Price { get; set; }

        public decimal SupplierBasePrice { get; set; }

        public string CurrencyCode { get; set; }
    }
}
