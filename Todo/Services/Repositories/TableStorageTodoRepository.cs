using Azure;
using Azure.Data.Tables;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TodoApi.Models.Entities;

namespace TodoApi.Services.Repositories
{
    class TableStorageTodoRepository : ITodoRepository
    {
        private readonly TableServiceClient tableServiceClient;
        private readonly TableClient tableClient;

        public TableStorageTodoRepository(TableServiceClient tableServiceClient)
        {
            this.tableServiceClient = tableServiceClient;
            tableClient = this.tableServiceClient.GetTableClient("Todos");
        }

        public async Task<IEnumerable<Todo>> FindAllTodosAsync()
        {
            var allTodos = await tableClient.QueryAsync<Todo>().ToListAsync();

            return allTodos;
        }

        public async Task<Todo> FindTodoByIdAsync(string id)
        {
            var todo = await tableClient.QueryAsync<Todo>(e => e.RowKey == id).FirstAsync();

            return todo;
        }

        public async Task InsertTodoAsync(Todo newTodo)
        {
            await tableClient.AddEntityAsync(newTodo);
        }

        public async Task RemoveTodoByIdAsync(string id)
        {
            var todo = await FindTodoByIdAsync(id);
            await tableClient.DeleteEntityAsync(todo.PartitionKey, todo.RowKey);
        }

        public async Task UpdateTodoAsync(Todo updatedTodo)
        {
            await tableClient.UpdateEntityAsync(updatedTodo, ETag.All, TableUpdateMode.Replace);
        }
    }
}
