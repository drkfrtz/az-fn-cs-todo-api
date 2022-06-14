using System.Net;
using System.Net.Http;

using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

using TodoApi.Extensions;
using TodoApi.Models.Dtos;
using TodoApi.Models.Entities;
using TodoApi.Services.Repositories;

namespace TodoApi
{
    public class CreateTodo
    {
        private readonly ITodoRepository repository;

        public CreateTodo(ITodoRepository repository)
        {
            this.repository = repository;
        }

        [OpenApiOperation(
            operationId: "CreateTodo",
            tags: new[] { "todos" },
            Summary = "Creat new Todo",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateTodoRequest))]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.Created,
            Description = "Todo successfully created"
        )]
        [Function(nameof(global::TodoApi.CreateTodo))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Post), Route = "todos")]
                HttpRequestData req
        )
        {
            var createTodoRequest = await req.ReadFromJsonAsync<CreateTodoRequest>();

            var newTodo = new Todo(createTodoRequest);

            await this.repository.InsertTodoAsync(newTodo);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add(
                HttpResponseHeader.Location.ToString(),
                $"{req.Url.GetBaseUrl()}/{newTodo.RowKey}"
            );
            return response;
        }
    }
}
