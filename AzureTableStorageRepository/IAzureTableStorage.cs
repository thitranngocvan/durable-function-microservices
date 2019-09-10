using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageRepository
{
    public interface IAzureTableStorage<T> where T : AzureTableEntity, new()
    {
        Task Delete(string partitionKey, string rowKey);
        Task<T> GetItem(string partitionKey, string rowKey);
        Task<List<T>> GetList();
        Task<List<T>> GetList(string partitionKey);
        Task Insert(T item);
        Task Update(T item);
        Task InsertManyAsync(T[] items);
    }

}
