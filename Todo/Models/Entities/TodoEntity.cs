using System;
using Azure;
using Azure.Data.Tables;
using TodoApi.Models.Dtos;

namespace TodoApi.Models.Entities
{
    public record TodoEntity : ITableEntity
    {
        public static string PARTITIONKEY_DATE_FORMAT = "yyyy-MM-dd";
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }

        public TodoEntity() { }

        public TodoEntity(CreateTodoRequest createTodoReq)
        {
            var timestamp = DateTimeOffset.Now.UtcDateTime;
            this.PartitionKey = timestamp.ToString(PARTITIONKEY_DATE_FORMAT);
            this.PartitionKey = this.RowKey = Guid.NewGuid().ToString("n");
            this.CreatedAt = timestamp;
            this.Description = createTodoReq.Description;
        }
    }
}
