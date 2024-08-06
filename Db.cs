//データアクセス層

namespace TaskManager.DB;
public record Task
{
    public int Id {get; set;}
    public string ? Name {get; set;} = string.Empty;
    public bool IsCompleted {get; set;}

    internal object OrderBy(Func<object, object> value)
    {
        throw new NotImplementedException();
    }
}

public class TaskDB
{
    private static List<Task> _tasks = new List<Task>()
    {
        new Task{ Id=1, Name="運動", IsCompleted = false},
        new Task{ Id=2, Name="掃除", IsCompleted = false},
        new Task{ Id=3, Name="読書", IsCompleted = true}
    };

    public static List<Task> GetTasks()
    {
        
        return _tasks;
    }
    
    public static Task ? GetTask(int id)
    {
        return _tasks.SingleOrDefault(task => task.Id == id);
    }
    
    public static Task CreateTask(Task task)
    {
        if(string.IsNullOrEmpty(task.Name))
            throw new ArgumentException("Task name cannot be empty");

        if(_tasks.Any(t => t.Id == task.Id))
            throw new ArgumentException("Task with the same ID already exists");
        _tasks.Add(task);
        return task;
    }

    public static Task UpdateTask(Task update)
    {
        if(string.IsNullOrEmpty(update.Name))
        throw new ArgumentException("Task name cannot be empty");

        _tasks = _tasks.Select(task =>
        {
            if (task.Id == update.Id)
            {
                task.Name = update.Name;
                task.IsCompleted = update.IsCompleted;
            }
            return task;
        }).ToList();
        return update;
    }

    public static void RemoveTask(int id)
    {
        _tasks = _tasks.FindAll(task => task.Id != id).ToList();
    }

    
}