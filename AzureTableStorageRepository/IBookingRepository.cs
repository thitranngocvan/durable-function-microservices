using AzureTableStorageRepository.Models;
using System;
using System.Threading.Tasks;

namespace AzureTableStorageRepository
{
    public interface IBookingRepository
    {
        Task<BookingResponseModel> AddBooking(BookingRequestModel model);

        Task<bool> CancelBooking(Guid bookingGuid, string cancelReason);

        Task<bool> ConfirmBooking(Guid bookingGuid);

        //Task<bool> CompletePayment()
    }
}
