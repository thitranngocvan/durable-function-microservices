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

        public async Task<bool> CancelBooking(Guid bookingGuid, string cancelReason)
        {
            return await _bookingRepository.CancelBooking(bookingGuid, cancelReason);
        }

        public async Task<bool> ConfirmBooking(Guid bookingGuid)
        {
            return await _bookingRepository.ConfirmBooking(bookingGuid);
        }
    }
}
