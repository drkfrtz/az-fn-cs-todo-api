using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.Dtos
{
    public record UpdateTodoRequest
    {
        public string Description { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
