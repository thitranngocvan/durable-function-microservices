using System;

namespace CarBooking.Models
{
    public class CarPrice
    {
        public Car Car { get; set; }

        public Supplier Supplier { get; set; }

        public decimal Price { get; set; }

        public decimal SupplierPrice { get; set; }

        public Guid PriceId { get; set; }

        public BookingDetail BookingDetail { get; set; }
    }
    public class CarSearchRequest
    {
        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string PickupLocation { get; set; }

        public string ReturnLocation { get; set; }

        public string CurrencyCode { get; set; }
    }
    public class BookingDetail
    {
        public Location PickupLocation { get; set; }

        public Location ReturnLocation { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }
    }
    public class Location : BaseModel { }
    public class CarClass : BaseModel
    {

    }

    public class SupplierCar
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CarClassId { get; set; }
        public string Doors { get; set; }
        public string Seats { get; set; }

        // public Supplier Supplier { get; set; }
    }

    public class SupplierCarPrice
    {
        public SupplierCar SupplierCar { get; set; }

        public decimal SupplierBasePrice { get; set; }

        public string Currency { get; set; }
    }
    public class Car : BaseModel
    {
        public string ImageUrl { get; set; }

        public int CarClassId { get; set; }

        public CarClass CarClass { get; set; }

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }
    }
    public class Supplier : BaseModel
    {
        public string LogoUrl { get; set; }
    }

    public class BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class BookingCommission
    {
        public CommissionTypeEnum CommissionType { get; set; }

        public decimal CommissionValue { get; set; }

        public string CurrencyCode { get; set; }
    }

    public enum CommissionTypeEnum
    {
        FixedMargin,
        PercentageMargin
    }

    public enum CurrencyEnum
    {
        USD,
        EUR,
        GBP
    }

    public enum SupplierEnum
    {
        Sixt,
        Goldcar,
        Budget
    }
}
