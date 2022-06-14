using System.Linq;
using System.Net;
using System.Net.Http;

using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

using TodoApi.Extensions;
using TodoApi.Models.Dtos;
using TodoApi.Services.Repositories;

namespace TodoApi
{
    public class GetTodos
    {
        private readonly ITodoRepository repository;

        public GetTodos(ITodoRepository repository)
        {
            this.repository = repository;
        }

        [OpenApiOperation(
            operationId: "GetTodos",
            tags: new[] { "todos" },
            Summary = "List all Todos",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(Todos)
        )]
        [Function(nameof(GetTodos))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "todos")]
                HttpRequestData req
        )
        {
            var todoList = await repository.FindAllTodosAsync();
            var todosResponse = new Todos(todoList.Select(todo => new TodoItem(todo)));

            return await req.CreateOkResponse<Todos>(todosResponse);
        }
    }
}
