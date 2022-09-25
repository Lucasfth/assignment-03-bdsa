namespace Assignment3.Entities;
using Assignment3.Core;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;

public class TagRepository : ITagRepository
{

    public Collection<Tag> tags = new Collection<Tag>();


    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        foreach (var t in tags)
        {
            if (t.Name.Equals(tag.Name)){
                return (Response.Conflict, t.Id);
            }
        }
        Tag tg = new Tag();
        tg.Name = tag.Name;
        return (Response.Created, tg.Id);
    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        var temp = new Collection<TagDTO>();
        
        foreach (var tag in tags)
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
                return tag;
            }
        }

        return new TagDTO(-1, "");
    }

    public Response Update(TagUpdateDTO tag)
    {   
        foreach (var t in tags)
        {   
            if (t.Id == tag.Id && !t.Name.Equals(tag.Name)){
                t.Name = tag.Name;
                return Response.Updated;
            }
        }  
        return Response.NotFound;
    }

    public Response Delete(int tagId, bool force = false)
    {
        foreach (var t in tags)
        {
            if(t.Id == tagId){
                tags.Remove(t);
                return Response.Deleted;
            }
        }
        return Response.NotFound;

    }
}
