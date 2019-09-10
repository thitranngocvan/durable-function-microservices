using System;

namespace AzureTableStorageRepository.Models
{
    public class BookingRequestModel
    {
        // Booking info
        //public BookingRequestDetail BookingRequest { get; set; }

        // CarPrice info
        public CarPrice CarPrice { get; set; }

        // Payment info
        public Payment PaymentInfo { get; set; }

        public Guid CustomerGuid { get; set; }

        public string DriverName { get; set; }

        public int DriverAge { get; set; }

        public BookingRequestDetail BookingDetail { get; set; }

    }
}
