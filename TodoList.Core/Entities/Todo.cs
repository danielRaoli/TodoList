using TodoList.Core.Enums;

namespace TodoList.Core.Entities
{
    public class Todo
    {
        
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;

        public TodoStatus IsComplete { get; set; }

        public Todo()
        {
            IsComplete = TodoStatus.Incomplete;
        }

        public void alternateStatus()
        {
            IsComplete = IsComplete == TodoStatus.Complete ? TodoStatus.Incomplete : TodoStatus.Complete ;
        }

    }
}
