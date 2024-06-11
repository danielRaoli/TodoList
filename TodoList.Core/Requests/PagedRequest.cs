namespace TodoList.Core.Requests
{
    public class PagedRequest
    {
        public int PageSize { get; set; } = 25;
        public int PageNumber { get; set; } = 1;

    }
}
