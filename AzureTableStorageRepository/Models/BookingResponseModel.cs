using System;

namespace AzureTableStorageRepository.Models
{
    public class BookingResponseModel
    {
        public Guid BookingGuid { get; set; }

        public string BookingStatus { get; set; }

        public string Message { get; set; }

        public string CustomerEmail { get; set; }

        public string DriverName { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string PickupLocation { get; set; }

        public string ReturnLocation { get; set; }

        public decimal CarPrice { get; set; }

        public string CarName { get; set; }
    }
}
