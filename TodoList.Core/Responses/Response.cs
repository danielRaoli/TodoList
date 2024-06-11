using System.Text.Json.Serialization;

namespace TodoList.Core.Responses
{
    public class Response<T>
    {
        private int _code;
        private T? data;

        public string Message { get; set; } = string.Empty;
        [JsonIgnore]
        public bool IsSuccess => _code >= 200 && _code <= 299;
        public T? Data { get; set; }

        [JsonConstructor]
        public Response()
        {
            _code = 200;
        }

        public Response(T? data,string message, int code ) 
        {
            Message = message;
            Data = data;
            _code = code;
        }


    }
}
