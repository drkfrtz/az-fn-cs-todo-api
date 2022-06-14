using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Exceptions;
using TodoApi.Models.Entities;

namespace TodoApi.Services.Repositories
{
    public class InMemoryTodoRepository : ITodoRepository
    {
        private static readonly DateTime MockDataTimestamp = DateTime.UtcNow;
        private readonly List<Todo> todoList =
            new()
            {
                new Todo
                {
                    PartitionKey = "2022-05-01",
                    RowKey = "11111111-1111-1111-1111-111111111111",
                    Description = "Wash the laundry",
                    CreatedAt = InMemoryTodoRepository.MockDataTimestamp,
                    Timestamp = InMemoryTodoRepository.MockDataTimestamp
                },
                new Todo
                {
                    PartitionKey = "2022-05-01",
                    RowKey = "22222222-2222-2222-2222-222222222222",
                    Description = "Shop Groceries",
                    CreatedAt = InMemoryTodoRepository.MockDataTimestamp,
                    Timestamp = InMemoryTodoRepository.MockDataTimestamp
                },
                new Todo
                {
                    PartitionKey = "2022-05-02",
                    RowKey = "33333333-3333-3333-3333-333333333333",
                    Description = "Swing vacuum",
                    CreatedAt = InMemoryTodoRepository.MockDataTimestamp,
                    Timestamp = InMemoryTodoRepository.MockDataTimestamp
                }
            };

        public async Task<IEnumerable<Todo>> FindAllTodosAsync()
        {
            return await Task.FromResult(todoList);
        }

        public async Task<Todo> FindTodoByIdAsync(string id)
        {
            var todo = todoList.Where(todo => todo.RowKey == id).SingleOrDefault();
            if (todo is null)
            {
                throw new TodoNotFoundException(id.ToString());
            }
            return await Task.FromResult(todo);
        }

        public async Task InsertTodoAsync(Todo newTodo)
        {
            todoList.Add(newTodo);
            await Task.CompletedTask;
        }

        public async Task RemoveTodoByIdAsync(string id)
        {
            var index = todoList.FindIndex(existingTodo => existingTodo.RowKey == id);
            if (index is -1)
            {
                throw new TodoNotFoundException(id.ToString());
            }
            todoList.RemoveAt(index);
            await Task.CompletedTask;
        }

        public Task UpdateTodoAsync(Todo updatedTodo)
        {
            var index = todoList.FindIndex(
                existingTodo => existingTodo.RowKey == updatedTodo.RowKey
            );
            if (index is -1)
            {
                throw new TodoNotFoundException(updatedTodo.RowKey);
            }
            todoList[index] = updatedTodo;
            return Task.CompletedTask;
        }
    }
}
