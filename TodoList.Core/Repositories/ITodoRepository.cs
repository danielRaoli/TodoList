using System.Runtime.InteropServices;
using TodoList.Core.Entities;
using TodoList.Core.Requests.Todo;
using TodoList.Core.Responses;

namespace TodoList.Core.Repositories
{
    public interface ITodoRepository
    {
        Task<Response<Todo?>> CreateTodo(CreateRequest request);
        Task<Response<Todo?>> UpdateTodo(UpdateRequest request);
        Task<Response<Todo?>> DeleteTodo(DeleteRequest request);
        Task<PagedResponse<List<Todo?>>> GetAllTodos(GetAllRequest request);
        Task<Response<Todo?>> AlternateStatus(CompletTodoRequest request);

    }
}
