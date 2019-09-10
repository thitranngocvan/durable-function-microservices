using System;

namespace AzureTableStorageRepository.Models
{
    public class CarSearchRequest
    {
        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string PickupLocation { get; set; }

        public string ReturnLocation { get; set; }

        public string CurrencyCode { get; set; }
    }
}
