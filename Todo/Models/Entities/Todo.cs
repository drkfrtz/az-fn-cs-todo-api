using System;
using Azure;
using Azure.Data.Tables;
using TodoApi.Models.Dtos;

namespace TodoApi.Models.Entities
{
    public record Todo : ITableEntity
    {
        private static readonly string PARTITIONKEY_DATE_FORMAT = "yyyy-MM-dd";
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }

        public Todo() { }

        public Todo(CreateTodoRequest createTodoRequest)
        {
            var timestamp = DateTimeOffset.Now.UtcDateTime;
            PartitionKey = timestamp.ToString(PARTITIONKEY_DATE_FORMAT);
            RowKey = Guid.NewGuid().ToString();
            CreatedAt = timestamp;
            Description = createTodoRequest.Description;
        }
    }
}
