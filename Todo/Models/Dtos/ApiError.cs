namespace TodoApi.Models.Dtos
{
    public record ApiError
    {
        public string Message { get; }

        public ApiError(string Message)
        {
            this.Message = Message;
        }
    }
}
