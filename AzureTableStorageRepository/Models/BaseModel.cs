namespace AzureTableStorageRepository.Models
{
    public class BaseModel: AzureTableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
