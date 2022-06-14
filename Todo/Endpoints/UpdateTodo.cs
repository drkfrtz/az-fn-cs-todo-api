using System;
using Azure.Core.Serialization;

using Microsoft.Azure.Functions.Worker;

using Microsoft.Azure.Functions.Worker.Http;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

using System.Net;

using System.Threading.Tasks;

using TodoApi.Exceptions;

using TodoApi.Extensions;

using TodoApi.Models.Dtos;

using TodoApi.Models.Entities;

using TodoApi.Services.Repositories;

using System.Linq;
using Microsoft.OpenApi.Models;
using System.Net.Http;

namespace TodoApi
{
    public class UpdateTodo
    {
        private readonly ITodoRepository repository;

        public UpdateTodo(ITodoRepository repository)
        {
            this.repository = repository;
        }

        [OpenApiOperation(
            operationId: "UpdateTodo",
            tags: new[] { "todos" },
            Summary = "Update Todo's description and/or completion status",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiParameter(
            name: "id",
            Description = "UUID",
            In = ParameterLocation.Path,
            Required = true,
            Type = typeof(string)
        )]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateTodoRequest))]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(TodoItem),
            Description = "Todo successfully updated"
        )]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.NotFound,
            Description = "Todo with given id not found"
        )]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.UnprocessableEntity,
            Description = "Request body malformed"
        )]
        [Function(nameof(UpdateTodo))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                nameof(HttpMethod.Patch),
                Route = "todos/{id}"
            )]
                HttpRequestData req,
            string id
        )
        {
            UpdateTodoRequest updateTodoRequest = await req.ReadFromJsonAsync<UpdateTodoRequest>();

            if (updateTodoRequest.IsNullOrEmpty())
            {
                return await req.CreateMalformedBodyResponse();
            }

            Todo entity;

            try
            {
                entity = await repository.FindTodoByIdAsync(id);
            }
            catch (TodoNotFoundException)
            {
                return await req.CreateResourceNotFoundResponse();
            }

            if (updateTodoRequest.IsCompleted is not null)
            {
                entity.CompletedAt = updateTodoRequest.IsCompleted.GetValueOrDefault()
                  ? DateTimeOffset.UtcNow
                  : null;
            }

            if (string.IsNullOrWhiteSpace(updateTodoRequest.Description) is false)
            {
                entity.Description = updateTodoRequest.Description;
            }

            await repository.UpdateTodoAsync(entity);

            var todo = new TodoItem(entity: entity);
            return await req.CreateOkResponse(todo);
        }
    }
}
