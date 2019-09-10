using System;

namespace AzureTableStorageRepository.Models
{
    public class Customer : BaseModel
    {
        public Guid CustomerGuid { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BoD { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
