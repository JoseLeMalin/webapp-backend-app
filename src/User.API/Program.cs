

using BackendAPI.User.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var conString =  builder.Configuration["ApplicationDbContext"] ??
     throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.");

await using var conn = new NpgsqlConnection(conString);
await using var dataSource = NpgsqlDataSource.Create(conString);

await conn.OpenAsync();

IHostEnvironment env = builder.Environment;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var withApiVersioning = builder.Services.AddApiVersioning();
var port = Environment.GetEnvironmentVariable("PORT") ?? "5195";

var app = builder.Build();


await using (var cmd = dataSource.CreateCommand("INSERT INTO data (some_field) VALUES ($1)"))
{
    cmd.Parameters.AddWithValue("Hello world");
    await cmd.ExecuteNonQueryAsync();
}

// Retrieve all rows
await using (var cmd = dataSource.CreateCommand("SELECT some_field FROM data"))
await using (var reader = await cmd.ExecuteReaderAsync())
{
    while (await reader.ReadAsync())
    {
        Console.WriteLine(reader.GetString(0));
    }
}



app.MapControllers(); // Implements in app the listed controllers in ./Controllers

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(p => p.Path = "/swagger/{documentName}/swagger.yaml"); // https://github.com/RicoSuter/NSwag/wiki/AspNetCore-Middleware#generate-specification-in-yaml-format
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "User API";
        config.Path = "/swagger";
        // config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocumentPath = "/swagger/{documentName}/swagger.yaml"; // https://github.com/RicoSuter/NSwag/wiki/AspNetCore-Middleware#generate-specification-in-yaml-format
        config.DocExpansion = "list";
    });
}

// Exception handler
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async httpContext =>
    {
        var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
        if (pds == null
            || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
        {
            // Fallback behavior
            await httpContext.Response.WriteAsync("Fallback: An error occurred.");
        }
    });
});
app.UseStatusCodePages(async statusCodeContext => await Results
                                                        .Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                                                        .ExecuteAsync(statusCodeContext.HttpContext));

RouteGroupBuilder exception = app.MapGroup("/exception");
exception.MapGet("/", () =>
{
    throw new InvalidOperationException("Sample Exception");
});


app.Run();


// app.Run($"http://0.0.0.0:{port}");
/*

RouteGroupBuilder todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapDelete("/{id}", DeleteTodo);

static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
}

static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync());
}

static async Task<IResult> GetTodo(int id, TodoDb db)
{
    System.Diagnostics.Debug.WriteLine("In the getter");
    return await db.Todos.FindAsync(id)
        is Todo todo
            ? TypedResults.Ok(new TodoItemDTO(todo))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(TodoItemDTO todoItemDTO, TodoDb db)
{
    var todoItem = new Todo
    {
        IsComplete = todoItemDTO.IsComplete,
        Name = todoItemDTO.Name
    };

    db.Todos.Add(todoItem);
    await db.SaveChangesAsync();

    todoItemDTO = new TodoItemDTO(todoItem);

    return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDTO);
}

static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return TypedResults.NotFound();

    todo.Name = todoItemDTO.Name;
    todo.IsComplete = todoItemDTO.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
} */
