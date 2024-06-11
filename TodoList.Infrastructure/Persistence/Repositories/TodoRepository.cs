using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Entities;
using TodoList.Core.Repositories;
using TodoList.Core.Requests.Todo;
using TodoList.Core.Responses;

namespace TodoList.Infrastructure.Persistence.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IMongoCollection<Todo> _todoCollection;
        private readonly IMongoCollection<BsonDocument> _countersCollection;
        public TodoRepository(IMongoDatabase database)
        {

            _todoCollection = database.GetCollection<Todo>("registers");
            _countersCollection = database.GetCollection<BsonDocument>("counters");
            
        }
        public async Task<Response<Todo?>> CreateTodo(CreateRequest request)
        {
            try
            {
                 int id = await GetNextSequenceValueAsync("productid");
                var todo = new Todo { Description = request.Description, Id = id };
                await _todoCollection.InsertOneAsync(todo);
                return new Response<Todo?>(todo, "Nova tarefa criada", 201);
            }
            catch (Exception ex)
            {
                return new Response<Todo?>(null, "Erro ao tentar criar tarefa", 500);
            }
        }

        public async Task<Response<Todo?>> DeleteTodo(DeleteRequest request)
        {
            try
            {


                var todo = await _todoCollection.Find(x => x.Id == request.Id).SingleOrDefaultAsync();
                if (todo is null)
                    return new Response<Todo?>(null, "Not Found", 404);

                await _todoCollection.DeleteOneAsync(x => x.Id == request.Id);

                return new Response<Todo?>(todo, "Tarefa removida com sucesso", 200);
            }
            catch (Exception ex)
            {
                return new Response<Todo?>(null, "Erro ao tentar excluir Tarefa", 500);
            }

        }

        public async Task<PagedResponse<List<Todo?>>> GetAllTodos(GetAllRequest request)
        {
            try
            {
                var todos = await _todoCollection.Find(c => true).Skip(request.PageSize * (request.PageNumber - 1)).Limit(request.PageSize).ToListAsync();
                return new PagedResponse<List<Todo?>>(todos, pageNumber: request.PageNumber, pageSize: request.PageSize);

            }
            catch (Exception ex)
            {
                return new PagedResponse<List<Todo?>>(null, "erro ao tentar recuperar tarefas", 500);
            }
        }

        public async Task<Response<Todo?>> UpdateTodo(UpdateRequest request)
        {
            try
            {
                var todoDb = await _todoCollection.Find(x => x.Id == request.Id).SingleOrDefaultAsync();
                if (todoDb is null)
                    return new Response<Todo?>(null, "NotFound", 404);

                todoDb.Description = request.Description;

                await _todoCollection.FindOneAndReplaceAsync(x => x.Id == request.Id, todoDb);
                return new Response<Todo?>(todoDb, "Tarefa atualizada", 200);


            }
            catch (Exception ex)
            {
                return new Response<Todo?>(null, "Erro ao tentar atualizar Tarefa", 500);
            }
        }

        public async Task<Response<Todo?>> AlternateStatus(CompletTodoRequest request)
        {
            try
            {
                var todo = await _todoCollection.Find(x => x.Id == request.Id).SingleOrDefaultAsync();
                if (todo is null)
                    return new Response<Todo?>(null, "Not Found", 404);

                todo.alternateStatus();

                _todoCollection.FindOneAndReplace(x => x.Id == request.Id, todo);
                return new Response<Todo?>(todo, "Status", 200);
            }
            catch (Exception ex)
            {
                return new Response<Todo?>(null, "Nao foi possivel completar a tarefa", 500);
            }
        }

        public async Task<int> GetNextSequenceValueAsync(string sequenceName)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", sequenceName);
            var update = Builders<BsonDocument>.Update.Inc("sequence_value", 1);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var sequenceDocument = await _countersCollection.FindOneAndUpdateAsync(filter, update, options);
            return sequenceDocument["sequence_value"].AsInt32;
        }
    }
}
