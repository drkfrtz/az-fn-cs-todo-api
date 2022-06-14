using System.Collections.Generic;

namespace TodoApi.Models.Dtos
{
    public record Todos
    {
        public IEnumerable<TodoItem> TodoItems { get; }

        public Todos(IEnumerable<TodoItem> todoItems) => TodoItems = todoItems;
    }
}
