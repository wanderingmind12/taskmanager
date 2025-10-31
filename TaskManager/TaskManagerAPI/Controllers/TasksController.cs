using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private static readonly List<TaskItem> _tasks = new();
        private static int _nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetAll()
        {
            return Ok(_tasks);
        }

        [HttpPost]
        public ActionResult<TaskItem> AddTask([FromBody] TaskItem task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            return CreatedAtAction(nameof(GetAll), new { id = task.Id }, task);
        }

        [HttpPut("{id}/toggle")]
        public IActionResult ToggleCompletion(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            task.IsCompleted = !task.IsCompleted;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            _tasks.Remove(task);
            return NoContent();
        }
    }
}
