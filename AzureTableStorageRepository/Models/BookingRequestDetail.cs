using System;

namespace AzureTableStorageRepository.Models
{
    public class BookingRequestDetail
    {
        public Location PickupLocation { get; set; }

        public Location ReturnLocation { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }
    }
}
