using AzureTableStorageRepository.Models;
using System;
using System.Threading.Tasks;

namespace AzureTableStorageRepository
{
    public class BookingAzureTableRepositor : IBookingRepository
    {
        AzureTableStorage<Booking> _bookingTable;
        AzureTableStorage<BookingCarPrice> _bookingCarPriceTable;
        public BookingAzureTableRepositor(AzureTableStorage<Booking> bookingTable, AzureTableStorage<BookingCarPrice> bookingCarPriceTable)
        {
            _bookingTable = bookingTable;
            _bookingCarPriceTable = bookingCarPriceTable;
        }

        public async Task<BookingResponseModel> AddBooking(BookingRequestModel model)
        {
            // check customer exist

            // add booking
            var bookingGuid = Guid.NewGuid();
            var bookingDateLocationInfo = model.BookingDetail;
            var booking = new Booking
            {
                BookingCarPriceGuid = model.CarPrice.PriceRateId,
                BookingStatus = BookingStatusEnum.Submitted.ToString(),
                CarId = model.CarPrice.Car.Id,
                PickupDate = bookingDateLocationInfo.PickupDate,
                ReturnDate = bookingDateLocationInfo.ReturnDate,
                PickupLocation = bookingDateLocationInfo.PickupLocation.Name,
                ReturnLocation = bookingDateLocationInfo.ReturnLocation.Name,
                CustomerGuid = model.CustomerGuid,
                CarName = model.CarPrice.Car.Name,
                DriverAge = model.DriverAge,
                DriverName = model.DriverName,
                RowKey = bookingGuid.ToString(),
                PartitionKey = Constants.DefaultPartitionKey,
                //SupplierId = model.CarPrice.Supplier.Id,
            };
            await _bookingTable.Insert(booking);

            // add carPrice
            var bookingPrice = new BookingCarPrice {
                RowKey = model.CarPrice.PriceRateId.ToString(),
                PartitionKey = Constants.DefaultPartitionKey,
                CarId = model.CarPrice.Car.Id,
                CarName = model.CarPrice.Car.Name,
                CurrencyCode = model.CarPrice.CurrencyCode,
                Price  = model.CarPrice.Price,
                SupplierBasePrice = model.CarPrice.SupplierPrice
            };
            await _bookingCarPriceTable.Insert(bookingPrice);

            return new BookingResponseModel { BookingGuid = bookingGuid, BookingStatus = booking.BookingStatus};
        }


        public async Task<bool> CancelBooking(Guid bookingGuid, string cancelReason)
        {
            var booking = await _bookingTable.GetItem(Constants.DefaultPartitionKey, bookingGuid.ToString());
            booking.BookingStatus = BookingStatusEnum.Cancelled.ToString();
            booking.CancelReason = cancelReason;
            await _bookingTable.Update(booking);
            return true;
        }

        public async Task<bool> ConfirmBooking(Guid bookingGuid)
        {
            var booking = await _bookingTable.GetItem(Constants.DefaultPartitionKey, bookingGuid.ToString());
            booking.BookingStatus = BookingStatusEnum.Confirmed.ToString();
            await _bookingTable.Update(booking);
            return true;
        }
    }
}
