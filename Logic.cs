
using TaskManager.DB;
using Task = TaskManager.DB.Task;

public class Logic{
    
    public static Task ? GetTask(int id){
        var task = TaskDB.GetTask(id);

        return task;
    }

    public static List<Task> GetTasks(){
        var tasks = TaskDB.GetTasks();

        return tasks;
    }

    public static Task CreateTask(Task task){
        var createdTask = TaskDB.CreateTask(task);

        return createdTask;
    }

    public static Task UpdateTask(Task update){
        var updatedTask = TaskDB.UpdateTask(update);

        return updatedTask;
    }

    public static void RemoveTask(int id){
        TaskDB.RemoveTask(id);
    }
}