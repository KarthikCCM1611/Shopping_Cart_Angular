using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repo;
        public TodoController(ITodoRepository repo) => _repo = repo;

        [HttpGet] 
        public IActionResult GetAll() => Ok(_repo.GetAll());


        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id) =>
            _repo.Get(id) is { } item ? Ok(item) : NotFound();

        
        [HttpPost]
        public IActionResult Create([FromBody] CreateDto dto)
        {
            var item = _repo.Add(new TodoItem(Guid.NewGuid(), dto.Title, false));
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }


        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateDto dto) =>
            _repo.Update(id, new TodoItem(id, dto.Title, dto.IsDone)) ? NoContent() : NotFound();


        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id) =>
            _repo.Delete(id) ? NoContent() : NotFound();

        
        public record CreateDto(string Title);
        
        public record UpdateDto(string Title, bool IsDone);
    }
}
