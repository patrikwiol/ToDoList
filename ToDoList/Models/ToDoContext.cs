using Microsoft.EntityFrameworkCore;

namespace ToDo.Models
{
	public class ToDoContext(DbContextOptions<ToDoContext> options) : DbContext(options)
	{
        public DbSet<ToDoList> Lists { get; set; } = null!;
		public DbSet<MainTask> Tasks { get; set; } = null!;
		public DbSet<SubTask> SubTasks { get; set; } = null!;
	}
}
