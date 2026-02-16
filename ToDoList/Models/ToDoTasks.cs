namespace ToDo.Models
{
	public class ToDoList
	{
		public int ID { get; set; }
		public string Name { get; set; } = "New List";
		public List<MainTask> Tasks { get; set; } = new();
	}

	public class MainTask
	{
		public int ID { get; set; }
		public int ListID { get; set; }
		public string? Name { get; set; }
		public string? Note { get; set; }
		public bool IsComplete { get; set; }
		public List<SubTask> SubTasks { get; set; } = new();
	}

	public class SubTask
	{
		public int ID { get; set; }
		public int MainTaskID { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsComplete { get; set; }
	}
}
