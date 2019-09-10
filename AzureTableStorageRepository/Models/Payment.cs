using System;

namespace AzureTableStorageRepository.Models
{
    public class Payment : BaseModel
    {
        public Guid PaymentGuid { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// Null, Completed, Failed
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// Cash, Card
        /// </summary>
        public string PaymentType { get; set; }
    }
}
