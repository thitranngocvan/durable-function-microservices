namespace AzureTableStorageRepository.Models
{
    public class BookingCommission
    {
        public CommissionTypeEnum CommissionType { get; set; }

        public decimal CommissionValue { get; set; }

        public string CurrencyCode { get; set; }
    }
}
