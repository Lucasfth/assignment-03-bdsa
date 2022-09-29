using System.Collections.ObjectModel;
using Assignment3.Core;
using NotImplementedException = System.NotImplementedException;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private KanbanContext _context;
    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }
    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        Collection<Tag> tags = new Collection<Tag>();
        foreach (var t in task.Tags)
        {
            tags.Add(new Tag(){Name = t});
        }
        var temp = new Task() { Title = task.Title, AssignedTo = new User() {Id = task.AssignedToId.Value, Name = "Mommy", Email = ""}, Description = task.Description, Tags = tags , State = State.New, Created = DateTime.UtcNow.Date, StateUpdated = DateTime.UtcNow.Date};
        _context.Tasks.Add(temp);
        _context.SaveChanges();
        return (Response.Created, temp.Id);
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var temp = new Collection<TaskDTO>();
        foreach (var task in _context.Tasks)
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
        foreach (var task in _context.Tasks)
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

        foreach (var task in _context.Tasks)
        {
            if (task.Tags != null)
            {
                var tags = new Collection<string>();
                bool shouldInsert = false;
                foreach (var t in task.Tags)
                {
                    tags.Add(t.Name);
                    if (t.Name.Equals(tag))
                        shouldInsert = true;
                }
                if (shouldInsert)
                    temp.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, tags, task.State));
            }
        }

        if (temp.Count == 0)
            return null;
        return new ReadOnlyCollection<TaskDTO>(temp);
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var temp = new Collection<TaskDTO>();

        foreach (var task in _context.Tasks)
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
                temp.Add(new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, tags, task.State));
        }

        if (temp.Count == 0) 
            return null;
        return new ReadOnlyCollection<TaskDTO>(temp);
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        var temp = new Collection<TaskDTO>();

        foreach (var task in _context.Tasks)
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
        foreach (var task in _context.Tasks)
        {
            var tags = new Collection<string>();
            if (task.Tags != null)
            {
                foreach (var t in task.Tags)
                {
                    tags.Add(t.Name);
                }
            }

            if (task.Id == taskId)
            {
               return new TaskDetailsDTO(task.Id, task.Title, task.Description, task.Created, task.AssignedTo.Name, tags, task.State, task.StateUpdated);
            }
        }

        return null;
    }
    public Response Update(TaskUpdateDTO task)
    {
        foreach (var t in _context.Tasks)
        {
            if(t.Id == task.Id)
            {
                t.Title = task.Title;
                t.AssignedTo.Id = task.AssignedToId.Value;
                t.Description = task.Description;
                
                var tags = new List<Tag>();
                foreach (var tag in task.Tags) {
                    Tag tempTag = new Tag();
                    tempTag.Name = tag;
                    tags.Add(tempTag);
                }
                t.Tags = tags;
                t.State = task.State;
                t.StateUpdated = DateTime.UtcNow.Date;

                return Response.Updated;
            }
        }
        return Response.NotFound;
    }
    public Response Delete(int taskId)
    {
        foreach (var t in _context.Tasks)
        {
            if(t.Id == taskId)
            {
                if (t.State == State.New)
                {
                    _context.Remove(t);
                    _context.SaveChanges();
                    return Response.Deleted;
                } else if (t.State == State.Active)
                {
                    t.State = State.Removed;
                    t.StateUpdated = DateTime.UtcNow.Date;
                    return Response.Updated;
                }
                else
                    return Response.Conflict;
            }
        }
        return Response.NotFound;
    }
}
