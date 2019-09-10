namespace AzureTableStorageRepository.Models
{
    public class Car : BaseModel
    {
        public string ImageUrl { get; set; }

        public int CarClassId { get; set; }

        //public CarClass CarClass { get; set; }

        public string Doors { get; set; }
        public string Seats { get; set; }

    }
}
