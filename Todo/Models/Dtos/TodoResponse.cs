using System;
using TodoApi.Models.Entities;

namespace TodoApi.Models.Dtos
{
    public record TodoResponse
    {
        // RowKey
        public string Id { get; private set; }

        // Timestamp
        public DateTime UpdatedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Description { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        public TodoResponse(TodoEntity todoEntity)
        {
            this.Id = todoEntity.RowKey;
            this.Description = todoEntity.Description;
            this.CreatedAt = todoEntity.CreatedAt.GetValueOrDefault().UtcDateTime;
            this.UpdatedAt = todoEntity.Timestamp.GetValueOrDefault().UtcDateTime;

            this.CompletedAt =
                todoEntity.CompletedAt == null ? null : todoEntity.CompletedAt.Value.UtcDateTime;
        }
    }
}
