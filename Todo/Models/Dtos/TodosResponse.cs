using System.Collections.Generic;

namespace TodoApi.Models.Dtos
{
    public record TodosResponse
    {
        public IEnumerable<TodoResponse> todos { get; }

        public TodosResponse(IEnumerable<TodoResponse> todos) => this.todos = todos;
    }
}
