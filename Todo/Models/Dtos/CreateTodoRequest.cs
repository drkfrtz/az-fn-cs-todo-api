namespace TodoApi.Models.Dtos
{
    public record CreateTodoRequest
    {
        public string Description { get; set; }
    }
}
