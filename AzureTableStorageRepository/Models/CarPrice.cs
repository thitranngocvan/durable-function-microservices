using System;

namespace AzureTableStorageRepository.Models
{
    //public class CarPriceSearchResult
    //{
    //    public List<CarPrice> PriceResults { get; set; }

    //    public Supplier Supplier { get; set; }
    //}
    public class CarPrice
    {
        public Car Car { get; set; }

        //public Supplier Supplier { get; set; }

        public decimal Price { get; set; }

        //public decimal SupplierPrice { get; set; }

        public Guid PriceRateId { get; set; }

        //public BookingRequestDetail BookingDetail { get; set; }

        public string CurrencyCode { get; set; }
    }
}
