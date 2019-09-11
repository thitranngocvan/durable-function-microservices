using AzureTableStorageRepository.Models;
using System;
using System.Threading.Tasks;

namespace AzureTableStorageRepository
{
    public interface IBookingRepository
    {
        Task<BookingResponseModel> AddBooking(BookingRequestModel model);

        Task<bool> CancelBooking(Guid bookingGuid, string cancelReason, string instanceId);

        Task<bool> ConfirmBooking(Guid bookingGuid, string instanceId);

        //Task<bool> CompletePayment()
    }
}
