using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoClassController : ControllerBase
    {
        //private readonly TodoServiceClass _service;

        //public TodoClassController(TodoServiceClass service)
        //{
        //    _service = service;
        //}

        private readonly IToDo _service;

        public TodoClassController(IToDo service)
        {
            _service = service;
        }


        //[HttpGet]
        //public IActionResult Get() => Ok(_service.GetAll());

        //[HttpPost]
        //public IActionResult Post([FromBody] TodoItemClass item)
        //{
        //    _service.Add(item);
        //    return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        //}

        //[HttpPut("{id}")]
        //public IActionResult Put(Guid id, TodoItemClass item)
        //{
        //    if (!_service.Update(id, item)) return NotFound();
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult Delete(Guid id)
        //{
        //    if (!_service.Delete(id)) return NotFound();
        //    return NoContent();
        //}

        ////[HttpDelete]
        ////public IActionResult Delete(Guid id)
        ////{
        ////    if (!_service.Delete(id)) return NotFound();
        ////    return NoContent();
        ////}

        [HttpGet("GetToDoLists")]
        public IActionResult GetToDoLists() => Ok(_service.GetAllToDoLists());

        [HttpPost("AddNewToDo")]
        public IActionResult AddNewToDo([FromBody] TodoItemClass item)
        {
            if(!_service.AddNewDo(item)) return BadRequest("A to-do item with the same name already exists.");
            return CreatedAtAction(nameof(GetToDoLists), new { id = item.Id }, item);
        }

        [HttpPut("UpdateToDo/{id}")]
        public IActionResult UpdateToDo(Guid id, TodoItemClass item)
        {
            if (!_service.UpdateToDo(id, item)) return NotFound();
            return NoContent();
        }

        [HttpDelete("DeleteToDo/{id}")]
        public IActionResult DeleteToDo(Guid id)
        {
            if (!_service.DeleteToDo(id)) return NotFound();
            return NoContent();
        }

    }
}
