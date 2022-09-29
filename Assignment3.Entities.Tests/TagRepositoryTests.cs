using System.Collections.ObjectModel;
using Assignment3.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities.Tests;

public class TagRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;

    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        
        Tag son1 = new Tag();
        Tag son2 = new Tag();
        Tag son3 = new Tag();
        Tag son4 = new Tag();
        son1.Name = "Son";
        son2.Name = "Sonn";
        son3.Name = "Sonny";
        son4.Name = "SonnyB";

        User user = new User() { Email = "mail", Name = "Bent" };
        Task task = new Task() { Title = "fixIt", AssignedTo = user, Created = DateTime.UtcNow.Date, Description = "fix it godamn", State = State.Active, StateUpdated = DateTime.UtcNow.Date };
        var col = new Collection<Task>();
        col.Add(task);

        son4.Tasks = col;
        
        context.Tags.AddRange(son1, son2, son3, son4);

        context.SaveChanges();
        
        _context = context;
        _repository = new TagRepository(_context);
    }
    
    private ICollection<TagDTO> tagCol()
    {
        Tag son1 = new Tag();
        Tag son2 = new Tag();
        Tag son3 = new Tag();
        Tag son4 = new Tag();
        son1.Name = "Son";
        son1.Id = 1;
        son2.Name = "Sonn";
        son2.Id = 2;
        son3.Name = "Sonny";
        son3.Id = 3;
        son4.Name = "SonnyB";
        son4.Id = 4;
        
        var temp = new Collection<TagDTO>();
        temp.Add(new TagDTO(son1.Id, son1.Name));
        temp.Add(new TagDTO(son2.Id, son2.Name));
        temp.Add(new TagDTO(son3.Id, son3.Name));
        temp.Add(new TagDTO(son4.Id, son4.Name));

        return temp;
    }

    [Fact]
    public void Create_Already_Existing_Tag_Should_Return_Conflict_And_Id()
    {
        // Arrange
        
        // Act
        var actual = _repository.Create(new TagCreateDTO("SonnyB"));
        
        // Assert
        Assert.Equal((Response.Conflict, 4), actual);
    }

    [Fact]
    public void Create_Does_Not_Exist_Tag_Should_Return_Created_And_Id()
    {
        // Arrange

        // Act
        var actual = _repository.Create(new TagCreateDTO("MyName"));
        
        // Assert
        Assert.Equal((Response.Created, 5), actual);
    }

    [Fact]
    public void ReadAll_Should_Return_ReadCollection_Of_All_Tasks_Tags()
    {
        // Arrange
        var expected = tagCol();

        // Act
        var actual = _repository.ReadAll();
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Read_Should_Only_Return_Son1_Given1()
    {
         // Arrange

         // Act
        var actual = _repository.Read(1);
        
        // Assert
        Assert.Equal(new TagDTO(1, "Son"), actual);
    }

     [Fact]
    public void Update_Should_Respond_Updated_If_Name_Is_New()
    {
        // Arrange

        // Act
        var actual = _repository.Update(new TagUpdateDTO(1, "SonTheMan"));

        // Assert
        Assert.Equal(Response.Updated, actual);
    }

       [Fact]
    public void Update_Should_Respond_BadRequest_If_None_Is_Updated()
    {
        // Arrange

        // Act
        var actual = _repository.Update(new TagUpdateDTO(1, "Son"));
        // Assert
        Assert.Equal(Response.Conflict, actual);
    }

    [Fact]
    public void Delete_Should_Respond_Deleted()
    {
        // Arrange

        // Act
        var actual = _repository.Delete(1);
    
        // Assert
        Assert.Equal(Response.Deleted, actual);
    }

    [Fact]
    public void Delete_Should_Respond_Deleted_Using_Force()
    {
        // Arrange
        // Act
        var actual = _repository.Delete(4, true);
        
        // Assert
        Assert.Equal(Response.Deleted, actual);
    }

    [Fact]
    public void Delete_Should_Respond_Conflict_When_Not_Given_Force()
    {
        // Arrange
        // Act
        var actual = _repository.Delete(4);
        
        // Assert
        Assert.Equal(Response.Conflict, actual);
    }
    
}
