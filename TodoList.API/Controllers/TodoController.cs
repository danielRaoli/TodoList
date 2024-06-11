using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TodoList.Core.Entities;
using TodoList.Core.Repositories;
using TodoList.Core.Requests.Todo;
using TodoList.Core.Responses;

namespace TodoList.API.Controllers
{
    [ApiController]
    [Route("v1/todo-list")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;
        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }



        [HttpPost]
        [Produces(typeof(Response<Todo?>))]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody]CreateRequest request)
        {
          var result = await _repository.CreateTodo(request);
          return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Produces(typeof(PagedResponse<Todo?>))]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll([FromQuery]int pageSize, [FromQuery] int pageNumber)
        {
            var request = new GetAllRequest { PageSize = pageSize, PageNumber = pageNumber };
 
            var result = await _repository.GetAllTodos(request);
            return result.IsSuccess ?  Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        [Produces(typeof(Response<Todo?>))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update([FromRoute]int id, UpdateRequest request)
        {
            request.Id = id;
            var result = await _repository.UpdateTodo(request);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpDelete("{id}")]
        [Produces(typeof(Response<Todo?>))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var request = new DeleteRequest { Id = id }; 

            var result = await _repository.DeleteTodo(request);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> AlternateStatus([FromRoute] int id)
        {
            var request = new CompletTodoRequest { Id = id };

            var result =await  _repository.AlternateStatus(request);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
