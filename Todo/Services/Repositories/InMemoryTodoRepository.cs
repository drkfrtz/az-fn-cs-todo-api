using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Exceptions;
using TodoApi.Models.Dtos;
using TodoApi.Models.Entities;

namespace TodoApi.Services.Repositories
{
    public class InMemoryTodoRepository : ITodoRepository
    {
        private static DateTime mockDataTimestamp = DateTime.UtcNow;
        private readonly List<TodoEntity> items = new List<TodoEntity>()
        {
            new TodoEntity
            {
                PartitionKey = "",
                RowKey = Guid.NewGuid().ToString("n"),
                Description = "Wash the laundry",
                CreatedAt = InMemoryTodoRepository.mockDataTimestamp,
                Timestamp = InMemoryTodoRepository.mockDataTimestamp
            },
            new TodoEntity
            {
                PartitionKey = "",
                RowKey = Guid.NewGuid().ToString("n"),
                Description = "Shop Groceries",
                CreatedAt = InMemoryTodoRepository.mockDataTimestamp,
                Timestamp = InMemoryTodoRepository.mockDataTimestamp
            },
            new TodoEntity
            {
                PartitionKey = "",
                RowKey = Guid.NewGuid().ToString("n"),
                Description = "Swing vacuum",
                CreatedAt = InMemoryTodoRepository.mockDataTimestamp,
                Timestamp = InMemoryTodoRepository.mockDataTimestamp
            }
        };

        public string CreateTodo(CreateTodoRequest createTodoReq)
        {
            var entity = new TodoEntity(createTodoReq);
            this.items.Append<TodoEntity>(entity);
            return entity.RowKey;
        }

        public void DeleteTodo(string id)
        {
            TodoEntity entity = this.findTodoEntityById(id);
            this.items.Remove(entity);
        }

        public void UpdateTodo(string id, bool isCompleted)
        {
            var entity = this.findTodoEntityById(id);
            entity.Timestamp = DateTime.UtcNow;
            entity.CompletedAt = isCompleted ? DateTime.UtcNow : null;
        }

        public void UpdateTodo(string id, string description)
        {
            var entity = this.findTodoEntityById(id);
            entity.Timestamp = DateTime.UtcNow;
            entity.Description = description;
        }

        public void UpdateTodo(string id, string description, bool isCompleted)
        {
            var entity = this.findTodoEntityById(id);
            entity.Timestamp = DateTime.UtcNow;
            entity.Description = description;
            entity.CompletedAt = isCompleted ? DateTime.UtcNow : null;
        }

        public TodosResponse FindAllTodos()
        {
            return new TodosResponse(this.items.Select(e => new TodoResponse(e)));
        }

        public TodoResponse FindTodoById(string id)
        {
            TodoEntity entity = this.findTodoEntityById(id);

            return new TodoResponse(entity);
        }

        private TodoEntity findTodoEntityById(string id)
        {
            TodoEntity entity = this.items.Find(e => e.RowKey.Equals(id));

            if (entity == null)
            {
                throw new TodoNotFoundException(id.ToString());
            }
            return entity;
        }
    }
}
