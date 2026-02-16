using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController(ToDoContext context) : ControllerBase
    {
        // GET: api/ToDoList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoList>>> GetLists()
        {
            // Include Tasks and their SubTasks for a full data tree
            return await context.Lists.Include(l => l.Tasks).ThenInclude(t => t.SubTasks).ToListAsync();
        }

        // GET: api/ToDoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoList>> GetToDoList(int id)
        {
            var toDoList = await context.Lists.Include(l => l.Tasks).ThenInclude(t => t.SubTasks).AsNoTracking().FirstOrDefaultAsync(l => l.ID == id);

            if (toDoList == null)
                return NotFound();

            return toDoList;
        }

        // POST: api/ToDoList
        [HttpPost]
        public async Task<ActionResult<ToDoList>> PostToDoList(ToDoList toDoList)
        {
            context.Lists.Add(toDoList);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetToDoList), new { id = toDoList.ID }, toDoList);
        }

        // PUT: api/ToDoList/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoList(int id, ToDoList toDoList)
        {
            if (id != toDoList.ID)
                return BadRequest();

            context.Entry(toDoList).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Lists.Any(e => e.ID == id))
                    return NotFound();

                throw;
            }
            return NoContent();
        }

        // DELETE: api/ToDoList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoList(int id)
        {
            var toDoList = await context.Lists.FindAsync(id);
            if (toDoList == null)
                return NotFound();

            context.Lists.Remove(toDoList);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}