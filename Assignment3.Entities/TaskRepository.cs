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
        var temp = new Collection<TaskDTO>();

        foreach (var task in tasks)
        {
            var tags = new Collection<string>();
            if (task.Tags != null)
            {
                foreach (var t in task.Tags)
                {
                    tags.Add(t.Name);
                }
            }

            if (task.AssignedTo.Id == userId)
            {
                temp.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, tags, task.State));
            }
        }

        return new ReadOnlyCollection<TaskDTO>(temp);
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        var temp = new Collection<TaskDTO>();

        foreach (var task in tasks)
        {
            var tags = new Collection<string>();
            if (task.Tags != null)
            {
                foreach (var t in task.Tags)
                {
                    tags.Add(t.Name);
                }
            }

            if (task.State == state)
            {
                temp.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, tags, task.State));
            }
        }

        return new ReadOnlyCollection<TaskDTO>(temp);
    }
        
    public TaskDetailsDTO Read(int taskId)
    {
        //foreach (var task in tasks)
        //{
        //    var tags = new Collection<string>();
        //    if (task.Tags != null)
        //    {
        //        foreach (var t in task.Tags)
        //        {
        //            tags.Add(t.Name);
        //        }
        //    }

        //    if (task.Id == taskId)
        //    {
        //       return new TaskDetailsDTO(task.Id, task.Title, task.Description, DateTime.Now, task.AssignedTo.Name, tags, task.State, DateTime.Now);
        //    }
        //}
        throw new NotImplementedException();
    }
    public Response Update(TaskUpdateDTO task)
    {
        foreach (var t in tasks)
        {
            if(t.Id == task.Id)
            {
                t.Title = task.Title;
                t.AssignedTo.Id = task.AssignedToId.Value;
                t.Description ??= task.Description;
                
                var tags = new Collection<Tag>();
                foreach (var tag in task.Tags) {
                    Tag tempTag = new Tag();
                    tempTag.Name = tag;
                    tags.Add(tempTag);
                }
                t.Tags = tags;
                t.State = task.State;

                return Response.Updated;
            }
        }
        return Response.NotFound;
    }
    public Response Delete(int taskId)
    {
        throw new NotImplementedException();
    }
}
