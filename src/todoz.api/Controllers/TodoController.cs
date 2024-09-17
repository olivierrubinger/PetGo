using Microsoft.AspNetCore.Mvc;

using todoz.api.Models;
using todoz.api.Repositories;

namespace todoz.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todozRepository;


        public TodoController(ITodoRepository todozRepository)
        {
            _todozRepository = todozRepository;
        }

        public TodoController()
        {
            _todozRepository = new TodoInMemory();
        }

        [HttpGet]
        public ActionResult<List<Todo>> GetAll()
        {
            var todos = _todozRepository.GetAll();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public ActionResult<Todo> Get(int id)
        {
            var todo = _todozRepository.Get(id);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        public ActionResult Create(Todo todo)
        {
            _todozRepository.Add(todo);
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Todo todo)
        {
            if (id != todo.Id)
                return BadRequest();

            var existingTodo = _todozRepository.Get(id);
            if (existingTodo is null)
                return NotFound();

            _todozRepository.Update(todo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var todo = _todozRepository.Get(id);

            if (todo is null)
                return NotFound();

            _todozRepository.Delete(id);

            return NoContent();
        }
    }
}
