using System.Collections.ObjectModel;
using Assignment3.Core;
using Microsoft.VisualBasic;

namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{
    [Fact]
    public void Read_All_Should_Show_All_Tasks()
    {
        // Arrange
        TaskRepository taskRepo = new TaskRepository();
        Task task1 = new Task();
        Task task2 = new Task();
        Task task3 = new Task();
        Task task4 = new Task();

        task1.Description = "Her kommer";
        task2.Description = "Pippi Langstromp";
        task3.Description = "Chula hop";
        task4.Description = "Chula hey";
        task1.Id = 1;
        task2.Id = 2;
        task3.Id = 3;
        task4.Id = 4;
        task1.State = State.Active;
        task2.State = State.Closed;
        task3.State = State.New;
        task4.State = State.Resolved;
        task1.Title = "strofe1";
        task2.Title = "strofe2";
        task3.Title = "strofe3";
        task4.Title = "strofe4";
        User user1 = new User();
        User user2 = new User();
        User user3 = new User();
        User user4 = new User();
        user1.Name = "Bank";
        user2.Name = "Silas";
        user3.Name = "Lucas";
        user4.Name = "MyName";
        task1.AssignedTo = user1;
        task2.AssignedTo = user2;
        task3.AssignedTo = user3;
        task4.AssignedTo = user4;
        Tag tag1 = new Tag();
        Tag tag2 = new Tag();
        Tag tag3 = new Tag();
        tag1.Name = "Fix";
        tag2.Name = "Change";
        tag3.Name = "Add";
        var tags = new Collection<Tag>();
        var stringTags = new Collection<string>();
        tags.Add(tag1);
        tags.Add(tag2);
        tags.Add(tag3);
        stringTags.Add(tag1.Name);
        stringTags.Add(tag2.Name);
        stringTags.Add(tag3.Name);
        task1.Tags = tags;
        task3.Tags = tags;
        
        taskRepo.tasks.Add(task1);
        taskRepo.tasks.Add(task2);
        taskRepo.tasks.Add(task3);
        taskRepo.tasks.Add(task4);
        
        var t = new Collection<TaskDTO>();
        t.Add(new TaskDTO(task1.Id, task1.Title, task1.AssignedTo.Name, stringTags, task1.State));
        t.Add(new TaskDTO(task2.Id, task2.Title, task2.AssignedTo.Name, new Collection<string>(), task2.State));
        t.Add(new TaskDTO(task3.Id, task3.Title, task3.AssignedTo.Name, stringTags, task3.State));
        t.Add(new TaskDTO(task4.Id, task4.Title, task4.AssignedTo.Name, new Collection<string>(), task4.State));

        var expected = new ReadOnlyCollection<TaskDTO>(t);
        
        // Act
        var actual = taskRepo.ReadAll();
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadAllRemoved_Should_Only_Read_Removed_Which_None_Is()
    {
        // Arrange
        TaskRepository taskRepo = new TaskRepository();
        Task task1 = new Task();
        Task task2 = new Task();
        Task task3 = new Task();
        Task task4 = new Task();

        task1.Description = "Her kommer";
        task2.Description = "Pippi Langstromp";
        task3.Description = "Chula hop";
        task4.Description = "Chula hey";
        task1.Id = 1;
        task2.Id = 2;
        task3.Id = 3;
        task4.Id = 4;
        task1.State = State.Active;
        task2.State = State.Closed;
        task3.State = State.New;
        task4.State = State.Resolved;
        task1.Title = "strofe1";
        task2.Title = "strofe2";
        task3.Title = "strofe3";
        task4.Title = "strofe4";
        User user1 = new User();
        User user2 = new User();
        User user3 = new User();
        User user4 = new User();
        user1.Name = "Bank";
        user2.Name = "Silas";
        user3.Name = "Lucas";
        user4.Name = "MyName";
        task1.AssignedTo = user1;
        task2.AssignedTo = user2;
        task3.AssignedTo = user3;
        task4.AssignedTo = user4;
        Tag tag1 = new Tag();
        Tag tag2 = new Tag();
        Tag tag3 = new Tag();
        tag1.Name = "Fix";
        tag2.Name = "Change";
        tag3.Name = "Add";
        var tags = new Collection<Tag>();
        var stringTags = new Collection<string>();
        tags.Add(tag1);
        tags.Add(tag2);
        tags.Add(tag3);
        stringTags.Add(tag1.Name);
        stringTags.Add(tag2.Name);
        stringTags.Add(tag3.Name);
        task1.Tags = tags;
        task3.Tags = tags;
        
        taskRepo.tasks.Add(task1);
        taskRepo.tasks.Add(task2);
        taskRepo.tasks.Add(task3);
        taskRepo.tasks.Add(task4);
        
        var t = new Collection<TaskDTO>();

        var expected = new ReadOnlyCollection<TaskDTO>(t);
        
        // Act
        var actual = taskRepo.ReadAllRemoved();
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ReadAllRemoved_Should_Only_Read_Removed_Which_Two_Are()
    {
        // Arrange
        TaskRepository taskRepo = new TaskRepository();
        Task task1 = new Task();
        Task task2 = new Task();
        Task task3 = new Task();
        Task task4 = new Task();

        task1.Description = "Her kommer";
        task2.Description = "Pippi Langstromp";
        task3.Description = "Chula hop";
        task4.Description = "Chula hey";
        task1.Id = 1;
        task2.Id = 2;
        task3.Id = 3;
        task4.Id = 4;
        task1.State = State.Removed;
        task2.State = State.Closed;
        task3.State = State.New;
        task4.State = State.Removed;
        task1.Title = "strofe1";
        task2.Title = "strofe2";
        task3.Title = "strofe3";
        task4.Title = "strofe4";
        User user1 = new User();
        User user2 = new User();
        User user3 = new User();
        User user4 = new User();
        user1.Name = "Bank";
        user2.Name = "Silas";
        user3.Name = "Lucas";
        user4.Name = "MyName";
        task1.AssignedTo = user1;
        task2.AssignedTo = user2;
        task3.AssignedTo = user3;
        task4.AssignedTo = user4;
        Tag tag1 = new Tag();
        Tag tag2 = new Tag();
        Tag tag3 = new Tag();
        tag1.Name = "Fix";
        tag2.Name = "Change";
        tag3.Name = "Add";
        var tags = new Collection<Tag>();
        var stringTags = new Collection<string>();
        tags.Add(tag1);
        tags.Add(tag2);
        tags.Add(tag3);
        stringTags.Add(tag1.Name);
        stringTags.Add(tag2.Name);
        stringTags.Add(tag3.Name);
        task1.Tags = tags;
        task3.Tags = tags;
        
        taskRepo.tasks.Add(task1);
        taskRepo.tasks.Add(task2);
        taskRepo.tasks.Add(task3);
        taskRepo.tasks.Add(task4);
        
        var t = new Collection<TaskDTO>();
        t.Add(new TaskDTO(task1.Id, task1.Title, task1.AssignedTo.Name, stringTags, State.Removed));
        t.Add(new TaskDTO(task4.Id, task4.Title, task4.AssignedTo.Name, new Collection<string>(), State.Removed));

        var expected = new ReadOnlyCollection<TaskDTO>(t);
        
        // Act
        var actual = taskRepo.ReadAllRemoved();
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ReadAllByTag_Should_Only_Return_Tasks_Which_Holds_The_Tags()
    {
        // Arrange
        TaskRepository taskRepo = new TaskRepository();
        Task task1 = new Task();
        Task task2 = new Task();
        Task task3 = new Task();
        Task task4 = new Task();

        task1.Description = "Her kommer";
        task2.Description = "Pippi Langstromp";
        task3.Description = "Chula hop";
        task4.Description = "Chula hey";
        task1.Id = 1;
        task2.Id = 2;
        task3.Id = 3;
        task4.Id = 4;
        task1.State = State.Removed;
        task2.State = State.Closed;
        task3.State = State.New;
        task4.State = State.Removed;
        task1.Title = "strofe1";
        task2.Title = "strofe2";
        task3.Title = "strofe3";
        task4.Title = "strofe4";
        User user1 = new User();
        User user2 = new User();
        User user3 = new User();
        User user4 = new User();
        user1.Name = "Bank";
        user2.Name = "Silas";
        user3.Name = "Lucas";
        user4.Name = "MyName";
        task1.AssignedTo = user1;
        task2.AssignedTo = user2;
        task3.AssignedTo = user3;
        task4.AssignedTo = user4;
        Tag tag1 = new Tag();
        Tag tag2 = new Tag();
        Tag tag3 = new Tag();
        tag1.Name = "Fix";
        tag2.Name = "Change";
        tag3.Name = "Add";
        var tags = new Collection<Tag>();
        var stringTags = new Collection<string>();
        tags.Add(tag1);
        tags.Add(tag2);
        tags.Add(tag3);
        stringTags.Add(tag1.Name);
        stringTags.Add(tag2.Name);
        stringTags.Add(tag3.Name);
        task1.Tags = tags;
        task3.Tags = tags;
        
        taskRepo.tasks.Add(task1);
        taskRepo.tasks.Add(task2);
        taskRepo.tasks.Add(task3);
        taskRepo.tasks.Add(task4);
        
        var t = new Collection<TaskDTO>();
        t.Add(new TaskDTO(task1.Id, task1.Title, task1.AssignedTo.Name, stringTags, task1.State));
        t.Add(new TaskDTO(task3.Id, task3.Title, task3.AssignedTo.Name, stringTags, task3.State));

        var expected = new ReadOnlyCollection<TaskDTO>(t);
        
        // Act
        var actual = taskRepo.ReadAllByTag("Fix");
        
        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
