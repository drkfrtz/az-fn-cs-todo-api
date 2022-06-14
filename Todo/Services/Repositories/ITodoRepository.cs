using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TodoApi.Models.Dtos;
using TodoApi.Models.Entities;

namespace TodoApi.Services.Repositories
{
    public interface ITodoRepository
    {
        // Commands
        public Task InsertTodoAsync(Todo newTodo);
        public Task RemoveTodoByIdAsync(string id);
        public Task UpdateTodoAsync(Todo updatedTodo);

        // Queries
        public Task<IEnumerable<Todo>> FindAllTodosAsync();
        public Task<Todo> FindTodoByIdAsync(string id);
    }
}
