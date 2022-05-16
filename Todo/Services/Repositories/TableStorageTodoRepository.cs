using Azure.Data.Tables;
using System;
using System.Linq;
using TodoApi.Exceptions;
using TodoApi.Models.Dtos;
using TodoApi.Models.Entities;

namespace TodoApi.Services.Repositories
{
    class TableStorageTodoRepository : ITodoRepository
    {
        private TableClient tableClient { get; }
        private TableServiceClient tableServiceClient { get; }

        public TableStorageTodoRepository(
            TableServiceClient tableServiceClient,
            TableClient tableClient
        )
        {
            this.tableServiceClient = tableServiceClient;
            this.tableClient = tableClient;
        }

        public string CreateTodo(CreateTodoRequest createTodoReq)
        {
            var entity = new TodoEntity(createTodoReq);
            this.tableClient.AddEntity(entity);
            return entity.RowKey;
        }

        public void DeleteTodo(string id)
        {
            var entity = tableClient.Query<TableEntity>(filter: $"RowKey eq '{id}'").First();

            tableClient.DeleteEntity(entity.PartitionKey, entity.RowKey);
        }

        public TodosResponse FindAllTodos()
        {
            return new TodosResponse(
                tableClient.Query<TodoEntity>().Select(entity => new TodoResponse(entity))
            );
        }

        public TodoResponse FindTodoById(string id)
        {
            var entity = tableClient
                .Query<TodoEntity>(filter: $"RowKey eq '{id}'")
                .FirstOrDefault();
            ;

            if (entity == null)
            {
                throw new TodoNotFoundException();
            }

            return new TodoResponse(entity);
        }

        public void UpdateTodo(string id, string description)
        {
            var entity = tableClient
                .Query<TableEntity>(filter: $"RowKey eq '{id}'")
                .FirstOrDefault();
            ;
            entity["Description"] = description;

            this.tableClient.UpdateEntity(entity, entity.ETag);
        }

        public void UpdateTodo(string id, bool isCompleted)
        {
            var entity = tableClient
                .Query<TableEntity>(filter: $"RowKey eq '{id}'")
                .FirstOrDefault();
            ;

            entity["CompletedAt"] = isCompleted ? DateTime.UtcNow : null;

            this.tableClient.UpdateEntity(entity, entity.ETag, TableUpdateMode.Replace);
        }

        public void UpdateTodo(string id, string description, bool isCompleted)
        {
            var entity = tableClient
                .Query<TableEntity>(filter: $"RowKey eq '{id}'")
                .FirstOrDefault();
            ;
            entity["Description"] = description;

            entity["CompletedAt"] = isCompleted ? DateTime.UtcNow : null;

            this.tableClient.UpdateEntity(entity, entity.ETag, TableUpdateMode.Replace);
        }
    }
}
