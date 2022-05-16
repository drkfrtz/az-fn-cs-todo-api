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
using TodoApi.Services.Repositories;

namespace TodoApi
{
    public class TodoApi
    {
        private readonly ITodoRepository todoRepository;
        private readonly ObjectSerializer serializer;

        public TodoApi(ITodoRepository todoService, ObjectSerializer serializer)
        {
            this.todoRepository = todoService;
            this.serializer = serializer;
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
        [Function("CreateTodo")]
        public async Task<HttpResponseData> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos")] HttpRequestData req
        )
        {
            var createTodoReq = await req.ReadFromJsonAsync<CreateTodoRequest>();

            var todoId = this.todoRepository.CreateTodo(createTodoReq);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add(
                HttpResponseHeader.Location.ToString(),
                $"{req.Url.GetBaseUrl()}/{todoId}"
            );
            return response;
        }

        [OpenApiOperation(
            operationId: "GetTodos",
            tags: new[] { "todos" },
            Summary = "List all Todos",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(TodosResponse))]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.OK,
            Summary = "The response",
            Description = "This returns the response"
        )]
        [Function("GetTodos")]
        public async Task<HttpResponseData> GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequestData req
        )
        {
            var todosResponse = this.todoRepository.FindAllTodos();

            return await req.CreateOkResponse<TodosResponse>(todosResponse, serializer);
        }

        [OpenApiOperation(
            operationId: "GetTodoById",
            tags: new[] { "todos" },
            Summary = "Find a Todo by its id",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateTodoRequest))]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(TodoResponse),
            Description = "Todo found and returned"
        )]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.NotFound,
            Description = "Todo with given id not found"
        )]
        [Function("GetTodoById")]
        public async Task<HttpResponseData> GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos/{id}")]
                HttpRequestData req,
            string id
        )
        {
            try
            {
                TodoResponse todo = this.todoRepository.FindTodoById(id);

                return await req.CreateOkResponse<TodoResponse>(todo, serializer);
            }
            catch (TodoNotFoundException)
            {
                return await req.CreateResourceNotFoundResponse(serializer);
            }
        }

        [OpenApiOperation(
            operationId: "UpdateTodo",
            tags: new[] { "todos" },
            Summary = "Update Todo's description and/or completion status",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateTodoRequest))]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(TodoResponse),
            Description = "Todo updated and returned"
        )]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.NotFound,
            Description = "Todo with given id not found"
        )]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.UnprocessableEntity,
            Description = "Request body malformed"
        )]
        [Function("UpdateTodo")]
        public async Task<HttpResponseData> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "todos/{id}")]
                HttpRequestData req,
            string id
        )
        {
            UpdateTodoRequest updateTodoReq = await req.ReadFromJsonAsync<UpdateTodoRequest>();

            if (UpdateTodoRequest.IsNullOrEmpty(updateTodoReq))
            {
                return await req.CreateMalformedBodyResponse(serializer);
            }

            if (updateTodoReq.IsCompleted != null)
            {
                this.todoRepository.UpdateTodo(id, updateTodoReq.IsCompleted.Value);
            }

            if (updateTodoReq.Description != null)
            {
                this.todoRepository.UpdateTodo(id, updateTodoReq.Description);
            }

            TodoResponse todo = this.todoRepository.FindTodoById(id);

            return await req.CreateOkResponse<TodoResponse>(todo, serializer);
        }

        [OpenApiOperation(
            operationId: "DeleteTodo",
            tags: new[] { "todos" },
            Summary = "Delete a Todo",
            Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateTodoRequest))]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.NotFound,
            Description = "Todo with given id not found"
        )]
        [OpenApiResponseWithoutBody(
            statusCode: HttpStatusCode.NoContent,
            Description = "Todo successfully deleted"
        )]
        [Function("DeleteTodo")]
        public async Task<HttpResponseData> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todos/{id}")]
                HttpRequestData req,
            string id
        )
        {
            try
            {
                this.todoRepository.DeleteTodo(id);
                return req.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (TodoNotFoundException)
            {
                return await req.CreateResourceNotFoundResponse(serializer);
            }
        }
    }
}
