namespace Assignment3.Entities;
using Assignment3.Core;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;

public class TagRepository : ITagRepository
{
    
    public KanbanContext _context;
    
    public TagRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        foreach (var t in _context.Tags)
        {
            if (t.Name.Equals(tag.Name)){
                return (Response.Conflict, t.Id);
            }
        }
        Tag tg = new Tag(){Name = tag.Name};
        _context.Tags.Add(tg);
        _context.SaveChanges();
        return (Response.Created, tg.Id);
    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        var temp = new Collection<TagDTO>();
        
        foreach (var tag in _context.Tags)
        {
           temp.Add(new TagDTO(tag.Id, tag.Name));
        }

        return new ReadOnlyCollection<TagDTO>(temp);
    }

    public TagDTO Read(int tagId)
    {
        foreach (var tag in ReadAll())
        {
            if (tag.Id == tagId)
            {
                return new TagDTO(tag.Id, tag.Name);
            }
        }

        return null;
    }

    public Response Update(TagUpdateDTO tag)
    {   
        foreach (var t in _context.Tags)
        {   
            if (t.Id == tag.Id && !t.Name.Equals(tag.Name)){
                t.Name = tag.Name;
                return Response.Updated;
            } else
                return Response.Conflict;
        }  
        return Response.NotFound;
    }

    public Response Delete(int tagId, bool force = false)
    {
        foreach (var t in _context.Tags)
        {
            if (t.Id == tagId)
            {
                if (t.Tasks == null)
                {
                    _context.Remove(t);
                    _context.SaveChanges();
                    return Response.Deleted;
                } else if (t.Tasks.Count > 0 && force)
                {
                    _context.Remove(t);
                    _context.SaveChanges();
                    return Response.Deleted;
                }
                else
                    return Response.Conflict;
            }
        }
        return Response.NotFound;

    }
}
