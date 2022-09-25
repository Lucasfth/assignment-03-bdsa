using System.Collections.ObjectModel;
using Assignment3.Core;
using NotImplementedException = System.NotImplementedException;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    public Collection<Task> tasks = new Collection<Task>();
    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        throw new NotImplementedException();
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var temp = new Collection<TaskDTO>();
        foreach (var task in tasks)
        {
            var t = new Collection<string>();

            if (task.Tags != null)
            {
                foreach (var tag in task.Tags)
                {
                    t.Add(tag.Name);
                }
            }
            temp.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, t, task.State));
        }

        return new ReadOnlyCollection<TaskDTO>(temp);
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        var temp = new Collection<TaskDTO>();
        foreach (var task in tasks)
        {
            var t = new Collection<string>();
            
            if (task.State == State.Removed)
            {
                if (task.Tags != null)
                {
                    foreach (var tag in task.Tags)
                    {
                        t.Add(tag.Name);
                    }
                }
                temp.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, t, task.State));
            }
        }

        return new ReadOnlyCollection<TaskDTO>(temp);
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        var temp = new Collection<TaskDTO>();

        foreach (var task in tasks)
        {
            if (task.Tags != null)
            {
                var tags = new Collection<string>();
                bool shouldInsert = false;
                foreach (var t in task.Tags)
                {
                    tags.Add(t.Name);
                    if (t.Name == tag)
                    {
                        shouldInsert = true;
                    }
                }
                if (shouldInsert)
                    temp.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, tags, task.State));
            }
        }

        return new ReadOnlyCollection<TaskDTO>(temp);
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        throw new NotImplementedException();
    }
    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        throw new NotImplementedException();
    }
    public TaskDetailsDTO Read(int taskId)
    {
        throw new NotImplementedException();
    }
    public Response Update(TaskUpdateDTO task)
    {
        throw new NotImplementedException();
    }
    public Response Delete(int taskId)
    {
        throw new NotImplementedException();
    }
}
