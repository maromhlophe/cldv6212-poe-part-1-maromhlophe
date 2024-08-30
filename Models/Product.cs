using Azure;
using Azure.Data.Tables;

namespace cloud_2.Models
{
    public class Product : ITableEntity
    {
        public string name { get; set; }
        public string price { get; set; }
        public string category { get; set; }
        public string image { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public ETag ETag { get; set; } = ETag.All;
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.Now;
    }
}
