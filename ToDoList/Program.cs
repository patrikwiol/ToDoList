using Microsoft.EntityFrameworkCore;
using ToDo.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ToDoContext>(opt => opt.UseInMemoryDatabase("ToDoContext"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Setup HTML
app.UseDefaultFiles();
app.UseStaticFiles();

// Finish App Setup
app.UseAuthorization();
app.MapControllers();

app.Run();
