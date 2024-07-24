using TaskManager.DB;
using Microsoft.OpenApi.Models;
using Task = TaskManager.DB.Task;

var builder = WebApplication.CreateBuilder(args);

//CORSポリシーの追加
builder.Services.AddCors(options =>{
    options.AddPolicy("AllowReactApp",
    builder =>{
        builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManager API", Description = "Manage your tasks effectively", Version = "v1" });
});

var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI(c =>{
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager V1");
    });
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();

//CORSを有効にする
app.UseCors("AllowReactApp");

app.UseExceptionHandler("/error");
app.MapGet("/", () => "Hello World!");

app.MapGet("/tasks/{id}", (int id) => {
    var task = TaskDB.GetTask(id);
    if(task == null){
        logger.LogWarning("Task with id {id} not found", id);
        return Results.NotFound($"Task with id {id} not found");
    }
    return Results.Ok(task);
});
//昇順、降順の並び替えられるようにする
//app.MapGet("/tasks", () => TaskDB.GetTasks());
app.MapGet("/tasks", ()=>{
    var tasks = TaskDB.GetTasks();
    var orderTasks = tasks.OrderBy(task => task.Id);
    return orderTasks;
});
app.MapPost("/tasks", (Task task) =>
{
    try
    {
        var createdTask = TaskDB.CreateTask(task);
        return Results.Created($"/tasks/{createdTask.Id}", createdTask);
    }
    catch (ArgumentException ex)
    {
        logger.LogWarning(ex, "Error creating task");
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/tasks", (Task task) =>
{
    try
    {
        var updatedTask = TaskDB.UpdateTask(task);
        return Results.Ok(updatedTask);
    }
    catch (ArgumentException ex)
    {
        logger.LogWarning(ex, "Error updating task");
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/tasks/{id}", (int id) =>
{
    try
    {
        TaskDB.RemoveTask(id);
        return Results.NoContent();
    }
    catch (ArgumentException ex)
    {
        logger.LogWarning(ex, "Error deleting task");
        return Results.BadRequest(ex.Message);
    }
});

app.Map("/error", () => Results.Problem("An error occurred while processing your request"));

app.Run();
