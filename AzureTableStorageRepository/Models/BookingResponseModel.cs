using System;

namespace AzureTableStorageRepository.Models
{
    public class BookingResponseModel
    {
        public Guid BookingGuid { get; set; }

        public string BookingStatus { get; set; }

        public string Message { get; set; }

        public string CustomerEmail { get; set; }
    }
}
