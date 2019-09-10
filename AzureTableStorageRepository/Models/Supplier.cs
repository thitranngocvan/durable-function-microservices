namespace AzureTableStorageRepository.Models
{
    public class Supplier : BaseModel
    {
        //public string LogoUrl { get; set; }

        public bool IsDeactivated { get; set; }

        public string Website { get; set; }
    }
}
