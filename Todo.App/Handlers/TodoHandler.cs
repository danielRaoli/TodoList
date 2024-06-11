using System.Net.Http.Json;
using TodoList.Core.Repositories;
using TodoList.Core.Requests.Todo;
using TodoList.Core.Responses;
using TodoList.Core.Entities;

namespace Todo.App.Handlers
{
    public class TodoHandler : ITodoRepository
    {
        private readonly HttpClient _client;
        private const string Endpoint = "v1/todo-list";

        public TodoHandler(IHttpClientFactory httpFactory)
        {
            _client = httpFactory.CreateClient(WebConfiguration.HttpClientName);
        }
        public async Task<Response<TodoList.Core.Entities.Todo?>> AlternateStatus(CompletTodoRequest request)
        {
            var result = await _client.GetAsync(Endpoint+$"/{request.Id}");

            return await result.Content.ReadFromJsonAsync<Response<TodoList.Core.Entities.Todo?>>() ?? new Response<TodoList.Core.Entities.Todo?>(null,"não foi possivel alterar o status", 500);
        }

        public async Task<Response<TodoList.Core.Entities.Todo?>> CreateTodo(CreateRequest request)
        {
            var result = await _client.PostAsJsonAsync(Endpoint, request);

            return await result.Content.ReadFromJsonAsync<Response<TodoList.Core.Entities.Todo?>>() ?? new Response<TodoList.Core.Entities.Todo?>(null,"Erro ao tentar criar tarefa",500);
        }

        public async Task<Response<TodoList.Core.Entities.Todo?>> DeleteTodo(DeleteRequest request)
        {
            var result = await _client.DeleteAsync(Endpoint + $"/{request.Id}");
            return await result.Content.ReadFromJsonAsync<Response<TodoList.Core.Entities.Todo?>>() ?? new Response<TodoList.Core.Entities.Todo>(null, "Nao foi possivel excluir", 500);
        }

        public async Task<PagedResponse<List<TodoList.Core.Entities.Todo?>>> GetAllTodos(GetAllRequest request)
        {
            var result = await _client.GetAsync(Endpoint);
            return await result.Content.ReadFromJsonAsync<PagedResponse<List<TodoList.Core.Entities.Todo?>>>() ?? new PagedResponse<List<TodoList.Core.Entities.Todo?>>(null,"Nao foi possivel recuperar as tarefas",500);
        }

        public async Task<Response<TodoList.Core.Entities.Todo?>> UpdateTodo(UpdateRequest request)
        {
            var result = await _client.PutAsJsonAsync(Endpoint + $"/{request.Id}", request);

            return await result.Content.ReadFromJsonAsync<Response<TodoList.Core.Entities.Todo?>>() ?? new Response<TodoList.Core.Entities.Todo?>(null, "erro ao atualizar", 500);
        }
    }
}
