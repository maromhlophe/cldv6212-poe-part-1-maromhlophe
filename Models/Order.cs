using Azure;
using Azure.Data.Tables;

namespace cloud_2.Models
{
    public class Order: ITableEntity
    {
        public string product {  get; set; }
        public string PartitionKey { get; set; } = Guid.NewGuid().ToString();
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public ETag ETag { get; set; } = ETag.All;
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.Now;
    }
}
