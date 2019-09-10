using System;

namespace AzureTableStorageRepository.Models
{
    public class Booking : BaseModel
    {
        public Guid BookingGuid { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string PickupLocation { get; set; }
        public string ReturnLocation { get; set; }

        public Guid CustomerGuid { get; set; }
        public int DriverAge { get; set; }
        public string DriverName { get; set; }

        public int CarId { get;set; }

        public int SupplierId { get; set; }

        public string CarName { get; set; }

        /// <summary>
        /// Pending, Confirmed, Completed, Cancelled
        /// </summary>
        public string BookingStatus { get; set; }

        public string PaymentGuid { get; set; }

        /// <summary>
        /// retrieve pricing details of the booking
        /// </summary>
        public Guid BookingCarPriceGuid { get; set; }

        public string CancelReason { get; set; }
    }
}
