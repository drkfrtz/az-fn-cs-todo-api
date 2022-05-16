namespace TodoApi.Models.Dtos
{
    public record UpdateTodoRequest
    {
        public static bool IsNullOrEmpty(UpdateTodoRequest req)
        {
            return req == null || (req.Description == null && req.IsCompleted == null);
        }

        public string Description { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
