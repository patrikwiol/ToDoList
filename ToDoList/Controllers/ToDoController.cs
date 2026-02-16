using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController(ToDoContext context) : ControllerBase
    {
        // GET: api/ToDo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MainTask>> GetMainTask(int id)
        {
            var task = await context.Tasks.Include(t => t.SubTasks).FirstOrDefaultAsync(t => t.ID == id);
            if (task == null)
                return NotFound();

            return task;
        }

        // POST: api/ToDo
        [HttpPost]
        public async Task<ActionResult<MainTask>> PostMainTask(MainTask mainTask)
        {
            var parentList = await context.Lists.Include(l => l.Tasks).FirstOrDefaultAsync(l => l.ID == mainTask.ListID);
            if (parentList == null)
                return BadRequest("Parent list not found.");

            parentList.Tasks.Add(mainTask);
            context.Tasks.Add(mainTask);

            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMainTask), new { id = mainTask.ID }, mainTask);
        }

        // PUT: api/ToDo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMainTask(int id, MainTask mainTask)
        {
            if (id != mainTask.ID)
                return BadRequest();

            context.Entry(mainTask).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Tasks.Any(e => e.ID == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/ToDo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMainTask(int id)
        {
            var task = await context.Tasks.FirstOrDefaultAsync(t => t.ID == id);
            if (task == null)
                return NotFound();

            // Remove from Parent List's collection
            var parentList = await context.Lists.Include(l => l.Tasks).FirstOrDefaultAsync(l => l.ID == task.ListID);
            parentList?.Tasks.Remove(task);

            // Remove from Context
            context.Tasks.Remove(task);

            await context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/ToDo/subtask
        [HttpPost("subtask")]
        public async Task<ActionResult<SubTask>> PostSubTask(SubTask subTask)
        {
            // Find the Parent Task and include its subtasks
            var parentTask = await context.Tasks.Include(t => t.SubTasks).FirstOrDefaultAsync(t => t.ID == subTask.MainTaskID);
            if (parentTask == null)
                return BadRequest("Parent task not found.");

            context.SubTasks.Add(subTask);
            parentTask.SubTasks.Add(subTask);

            await context.SaveChangesAsync();
            return Ok(subTask);
        }

        // PUT: api/ToDo/subtask/5
        [HttpPut("subtask/{id}")]
        public async Task<IActionResult> PutSubTask(int id, SubTask subTask)
        {
            if (id != subTask.ID)
                return BadRequest();

            context.Entry(subTask).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.SubTasks.Any(e => e.ID == id))
                    return NotFound();

                throw;
            }
            return NoContent();
        }

        // DELETE: api/ToDo/subtask/5
        [HttpDelete("subtask/{id}")]
        public async Task<IActionResult> DeleteSubTask(int id)
        {
            var subTask = await context.SubTasks.FirstOrDefaultAsync(s => s.ID == id);
            if (subTask == null)
                return NotFound();

            // Remove from Parent Task's collection
            var parentTask = await context.Tasks.Include(t => t.SubTasks).FirstOrDefaultAsync(t => t.ID == subTask.MainTaskID);
            parentTask?.SubTasks.Remove(subTask);

            // Remove from Context
            context.SubTasks.Remove(subTask);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}