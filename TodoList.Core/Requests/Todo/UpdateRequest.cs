namespace TodoList.Core.Requests.Todo
{
    public class UpdateRequest
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
