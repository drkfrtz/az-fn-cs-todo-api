using TodoApi.Models.Dtos;

namespace TodoApi.Extensions
{
    public static class UpdateTodoRequestExtensions
    {
        public static bool IsNullOrEmpty(this UpdateTodoRequest updateTodoRequest)
        {
            return updateTodoRequest is null
                || (updateTodoRequest.Description is null && updateTodoRequest.IsCompleted is null);
        }
    }
}
