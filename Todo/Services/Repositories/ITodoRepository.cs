using TodoApi.Models.Dtos;

namespace TodoApi.Services.Repositories
{
    public interface ITodoRepository
    {
        // Commands
        public string CreateTodo(CreateTodoRequest createTodoReq);
        public void DeleteTodo(string id);
        public void UpdateTodo(string id, string description);
        public void UpdateTodo(string id, bool isCompleted);
        public void UpdateTodo(string id, string description, bool isCompleted);

        // Queries
        public TodosResponse FindAllTodos();
        public TodoResponse FindTodoById(string id);
    }
}
