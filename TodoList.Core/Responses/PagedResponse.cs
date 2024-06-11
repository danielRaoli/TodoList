using System.Text.Json.Serialization;

namespace TodoList.Core.Responses
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageSize { get; set; } = 1;
        public int PageNumber { get; set; } = 25;

        [JsonConstructor]
        public PagedResponse(T? data,int pageNumber = 1, int pageSize = 25) 
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PagedResponse(T? data, string message, int code) : base(data, message, code)
        {


        }
    }
}
