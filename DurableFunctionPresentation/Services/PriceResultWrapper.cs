using System.Collections.Generic;

namespace DurableFunctionPresentation.Services
{
    public class PriceResultWrapper
    {
        public List<PriceResult> PriceResults { get; set; }
    }
    public class PriceResult
    {
        public int SupplierId { get; set; }
        public decimal LocalPrice { get; set; }
        public string CarName { get; set; }
        public string CarImage { get; set; }
        public string CarClassId { get; set; }
        public string Doors { get; set; }
        public string Seats { get; set; }

        public decimal SupplierPrice { get; set; }

    }
}