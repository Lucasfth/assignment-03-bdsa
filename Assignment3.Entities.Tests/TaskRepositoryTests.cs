using System.Collections.ObjectModel;
using Assignment3.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;
    
    private DateTime time = new DateTime(2022, 12, 1);

    public TaskRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        User user1 = new User() {Name = "Bank", Email = "Mail"};
        User user2 = new User() {Name = "Silas", Email = "Mail2"};
        User user3 = new User() {Name = "Lucas", Email = "Mail3"};
        User user4 = new User() {Name = "MyName", Email = "Mail4"};
        var tags = Tags();
        var tags2 = Tags2();
        context.Users.AddRange(user1, user2, user3, user4);
        context.Tasks.AddRange(new Task() { Title = "taskSQL" , AssignedTo = user1, Description = "Get SQL set up for program", State = State.Active, Tags = tags, Created = time, StateUpdated = time}, 
            new Task() { Title = "taskAI", AssignedTo = user2,Description = "Set up AI for program", State = State.Closed, Tags = tags, Created = time, StateUpdated = time},
            new Task() { Title = "taskAlgorithm", AssignedTo = user3, Description = "Set up algorithm", State = State.New, Tags = tags, Created = time, StateUpdated = time},
            new Task() { Title = "taskSomething", AssignedTo = user4, Description = "Get something set up", State = State.Removed, Tags = tags2, Created = time, StateUpdated = time});
        context.SaveChanges();

        _context = context;
        _repository = new TaskRepository(_context);
    }

    private List<Tag> Tags()
    {
        Tag tag1 = new Tag();
        Tag tag2 = new Tag();
        Tag tag3 = new Tag();
        tag1.Name = "Fix";
        tag2.Name = "Change";
        tag3.Name = "Add";
        var tags = new List<Tag>();
        tags.Add(tag1);
        tags.Add(tag2);
        tags.Add(tag3);
        return tags;
    }

    private Collection<Tag> Tags2()
    {
        Tag tag = new Tag();
        tag.Name = "Help";
        var tags = new Collection<Tag>();
        tags.Add(tag);
        return tags;
    }

    private Collection<string> TagsString()
    {
        var tags = new Collection<string>();
        tags.Add("Fix");
        tags.Add("Change");
        tags.Add("Add");
        return tags;
    }

    private Collection<string> Tags2String()
    {
        var tags = new Collection<string>();
        tags.Add("Help");
        return tags;
    }

    [Fact]
    public void Create_Should_Return_Created()
    {
        // Arrange
        var tags = Tags2String();
        var t = new TaskCreateDTO("New one", 6, "Another one", tags);
        var expected = (Response.Created, 5);
        
        // Act
        var actual = _repository.Create(t);
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadAll_Should_Show_All_Tasks()
    {
        var tags = TagsString();
        var tags2 = Tags2String();
        
        var t = new Collection<TaskDTO>();
        t.Add(new TaskDTO(1, "taskSQL", "Bank", tags, State.Active));
        t.Add(new TaskDTO(2, "taskAI", "Silas", tags, State.Closed));
        t.Add(new TaskDTO(3, "taskAlgorithm", "Lucas", tags, State.New));
        t.Add(new TaskDTO(4, "taskSomething", "MyName", tags2, State.Removed));

        var expected = new ReadOnlyCollection<TaskDTO>(t);
        
        // Act
        var actual = _repository.ReadAll();
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadAllRemoved_Should_Only_Read_Removed_Which_One_Is()
    {
        // Arrange
        var tags = Tags2String();
        
        var t = new Collection<TaskDTO>();
        t.Add(new TaskDTO(4, "taskSomething", "MyName", tags ,State.Removed));
        
        var expected = new ReadOnlyCollection<TaskDTO>(t);
        
        // Act
        var actual = _repository.ReadAllRemoved();
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadAllByTag_Should_Only_Return_Tasks_Which_Holds_The_Tags()
    {
        // Arrange
        var tags = Tags2String();
        
        var t = new Collection<TaskDTO>();
        t.Add(new TaskDTO(4, "taskSomething", "MyName", tags, State.Removed));

        var expected = new ReadOnlyCollection<TaskDTO>(t);
        
        // Act
        var actual = _repository.ReadAllByTag("Help");
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadAllByUser_Should_Only_Return_User_3()
    {
        // Arrange
        var tags = TagsString();
        
        var t = new Collection<TaskDTO>();
        t.Add(new TaskDTO(3, "taskAlgorithm", "Lucas", tags, State.New));

        var expected = new ReadOnlyCollection<TaskDTO>(t);
        
        // Act
        var actual = _repository.ReadAllByUser(3);
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ReadAllByState_Should_Only_Return_task_2()
    {
        // Arrange
        var tags = TagsString();
        
        var t = new Collection<TaskDTO>();
        t.Add(new TaskDTO(2, "taskAI", "Silas", tags, State.Closed));

        var expected = new ReadOnlyCollection<TaskDTO>(t);
        
        // Act
        var actual = _repository.ReadAllByState(State.Closed);
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Read_Should_Only_Send_User_4_Back()
    {
        // Arrange
        var tags = Tags2String();
        var expected = new TaskDetailsDTO(4, "taskSomething", "Get something set up", time, "MyName", tags,
            State.Removed, time);

        // Act
        var actual = _repository.Read(4);
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Update_Should_Respond_Updated()
    {
        // Arrange
        var tags = Tags2String();
        var taskUpdate = new TaskUpdateDTO(3, "Updated", 10, "Definitely Updated", tags, State.Resolved);
        
        // Act
        var actual = _repository.Update(taskUpdate);
        
        // Assert
        Assert.Equal(Response.Updated, actual);
    }

    [Fact]
    public void Update_Should_Respond_NotFound()
    {
        // Arrange
        var tags = Tags2String();
        var taskUpdate = new TaskUpdateDTO(5, "Updated", 10, "Definitely Updated", tags, State.Resolved);
        
        // Act
        var actual = _repository.Update(taskUpdate);
        
        // Assert
        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public void Delete_Should_Respond_NotFound_Given_Id_5()
    {
        // Arrange
        
        // Act
        var actual = _repository.Delete(5);
        
        // Assert
        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public void Delete_Should_Respond_Deleted_Given_Id_3()
    {
        // Arrange
        // Act
        var actual = _repository.Delete(3);
        
        // Assert
        Assert.Equal(Response.Deleted, actual);
    }
}
