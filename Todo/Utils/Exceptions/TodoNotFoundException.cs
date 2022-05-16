using System;

namespace TodoApi.Exceptions
{
    [Serializable]
    public class TodoNotFoundException : Exception
    {
        public string TodoId { get; }

        public TodoNotFoundException() { }

        public TodoNotFoundException(string message) : base(message) { }

        public TodoNotFoundException(string message, Exception inner) : base(message, inner) { }

        public TodoNotFoundException(string message, string todoId) : this(message)
        {
            TodoId = todoId;
        }
    }
}
