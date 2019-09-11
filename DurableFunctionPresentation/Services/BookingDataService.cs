using AzureTableStorageRepository;
using AzureTableStorageRepository.Models;
using System;
using System.Threading.Tasks;

namespace DurableFunctionPresentation.Services
{
    public class CarBookingService
    {
        IBookingRepository _bookingRepository;
        public CarBookingService(IBookingRepository bookingRepo)
        {
            _bookingRepository = bookingRepo;
        }

        public async Task<BookingResponseModel> AddBooking(BookingRequestModel model)
        {
            return await _bookingRepository.AddBooking(model);
        }

        public async Task<bool> CancelBooking(Guid bookingGuid, string cancelReason, string instanceId)
        {
            return await _bookingRepository.CancelBooking(bookingGuid, cancelReason, instanceId);
        }

        public async Task<bool> ConfirmBooking(Guid bookingGuid, string instanceId)
        {
            return await _bookingRepository.ConfirmBooking(bookingGuid,instanceId);
        }
    }
}
