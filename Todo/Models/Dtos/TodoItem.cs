using System;
using TodoApi.Models.Entities;

namespace TodoApi.Models.Dtos
{
    public record TodoItem
    {
        // RowKey
        public string Id { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Description { get; init; }
        public DateTime? CompletedAt { get; init; }

        public TodoItem(Todo entity)
        {
            Id = entity.RowKey;
            Description = entity.Description;
            CreatedAt = entity.CreatedAt.UtcDateTime;
            CompletedAt = entity.CompletedAt?.UtcDateTime;
        }
    }
}
