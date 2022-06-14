using System.Net;
using System.Net.Http;

using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

using TodoApi.Exceptions;
using TodoApi.Extensions;
using TodoApi.Models.Dtos;
using TodoApi.Services.Repositories;

namespace TodoApi
{
    public class GetTodoById
    {
        private readonly ITodoRepository repository;

        public GetTodoById(ITodoRepository repository)
        {
            this.repository = repository;
        }

        [OpenApiOperation(
            operationId: "GetTodoById",
            tags: new[] { "todos" },
            Summary = "Find a Todo by its id",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiParameter(
            name: "id",
            Description = "UUID",
            In = ParameterLocation.Path,
            Required = true,
            Type = typeof(string)
        )]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(TodoItem),
            Description = "Todo found and returned"
        )]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.NotFound,
            Description = "Todo with given id not found"
        )]
        [Function(nameof(GetTodoById))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                nameof(HttpMethod.Get),
                Route = "todos/{id}"
            )]
                HttpRequestData req,
            string id
        )
        {
            try
            {
                var entity = await repository.FindTodoByIdAsync(id);
                var todo = new TodoItem(entity: entity);

                return await req.CreateOkResponse(todo);
            }
            catch (TodoNotFoundException)
            {
                return await req.CreateResourceNotFoundResponse();
            }
        }
    }
}
