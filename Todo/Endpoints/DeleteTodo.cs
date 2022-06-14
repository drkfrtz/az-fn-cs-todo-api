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
using TodoApi.Services.Repositories;

namespace TodoApi
{
    public class DeleteTodo
    {
        private readonly ITodoRepository repository;

        public DeleteTodo(ITodoRepository repository)
        {
            this.repository = repository;
        }

        [OpenApiOperation(
            operationId: "DeleteTodo",
            tags: new[] { "todos" },
            Summary = "Delete a Todo",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiParameter(
            name: "id",
            Description = "UUID",
            In = ParameterLocation.Path,
            Required = true,
            Type = typeof(string)
        )]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.NotFound,
            Description = "Todo with given id not found"
        )]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.NoContent,
            Description = "Todo successfully deleted"
        )]
        [Function(nameof(DeleteTodo))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                nameof(HttpMethod.Delete),
                Route = "todos/{id}"
            )]
                HttpRequestData req,
            string id
        )
        {
            try
            {
                await repository.RemoveTodoByIdAsync(id);
                return req.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (TodoNotFoundException)
            {
                return await req.CreateResourceNotFoundResponse();
            }
        }
    }
}
