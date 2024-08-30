using Azure;
using Azure.Data.Tables;

namespace cloud_2.Models
{
    public class User: ITableEntity
    {
        public string Username { get; set; }
        public string email  { get; set; }
        public string address { get; set; }
        public string contact{ get; set; }
        public string PartitionKey { get; set; }
        public string RowKey{ get; set; }
        public ETag ETag { get; set; } = ETag.All;
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.Now;
    }
}
