using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    c.RoutePrefix = string.Empty;
});

app.MapGet("/todoitems", async (TodoDb db) => await db.Todos.ToListAsync());

app.MapPost("/todoitems", async (TodoItem todo, TodoDb db) => {
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.Run();

class TodoItem {
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}

class TodoDb : DbContext {
    public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }
    public DbSet<TodoItem> Todos => Set<TodoItem>();
}