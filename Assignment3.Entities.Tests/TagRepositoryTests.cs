using System.Collections.ObjectModel;
using System.Security.Cryptography;
using Assignment3.Core;

namespace Assignment3.Entities.Tests;

public class TagRepositoryTests
{
    [Fact]
    public void Create_Already_Existing_Tag_Should_Return_Conflict_And_Id()
    {
        // Arrange
        TagRepository tagRepo= new TagRepository();
       
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

        tagRepo.tags.Add(son1);
        tagRepo.tags.Add(son2);
        tagRepo.tags.Add(son3);
        tagRepo.tags.Add(son4);
           
        
        // Act
        var output = tagRepo.Create(new TagCreateDTO("SonnyB"));
        
        // Assert
        Assert.Equal((Response.Conflict, 4), output);
    }

    [Fact]
    public void Create_Does_Not_Exist_Tag_Should_Return_Created_And_Id()
    {
        // Arrange
        TagRepository tagRepo= new TagRepository();
        
        // Act
        var output = tagRepo.Create(new TagCreateDTO("MyName"));
        
        // Assert
        Assert.Equal((Response.Created, 0), output);
    }

    [Fact]
    public void ReadAll_Should_Return_ReadCollection_Of_All_Tasks_Tags()
    {
        // Arrange
        TagRepository tagRepo = new TagRepository();

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

        tagRepo.tags.Add(son1);
        tagRepo.tags.Add(son2);
        tagRepo.tags.Add(son3);
        tagRepo.tags.Add(son4);
        
        var temp = new Collection<TagDTO>();
        temp.Add(new TagDTO(son1.Id, son1.Name));
        temp.Add(new TagDTO(son2.Id, son2.Name));
        temp.Add(new TagDTO(son3.Id, son3.Name));
        temp.Add(new TagDTO(son4.Id, son4.Name));

        var expected = new ReadOnlyCollection<TagDTO>(temp);
        // Act
        var output = tagRepo.ReadAll();
        
        // Assert
        Assert.Equal(expected, output);
    }

    [Fact]
    public void Read_Should_Only_Return_Son1_Given1()
    {
         // Arrange
        TagRepository tagRepo = new TagRepository();

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

        tagRepo.tags.Add(son1);
        tagRepo.tags.Add(son2);
        tagRepo.tags.Add(son3);
        tagRepo.tags.Add(son4);
        
        // Act
        var output = tagRepo.Read(1);
        
        // Assert
        Assert.Equal(new TagDTO(1, "Son"), output);
    }

     [Fact]
    public void Update_Should_Respond_Updated_If_Name_Is_New()
    {
        // Arrange
        TagRepository tagRepo = new TagRepository();


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

        tagRepo.tags.Add(son1);
        tagRepo.tags.Add(son2);
        tagRepo.tags.Add(son3);
        tagRepo.tags.Add(son4);

        
       

        // Act
        var actual = tagRepo.Update(new TagUpdateDTO(1, "SonTheMan"));
        // Assert
        Assert.Equal(Response.Updated, actual);
    }

       [Fact]
    public void Update_Should_Respond_BadRequest_If_None_Is_Updated()
    {
        // Arrange
        TagRepository tagRepo = new TagRepository();


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

        tagRepo.tags.Add(son1);
        tagRepo.tags.Add(son2);
        tagRepo.tags.Add(son3);
        tagRepo.tags.Add(son4);

        // Act
        var actual = tagRepo.Update(new TagUpdateDTO(1, "Son"));
        // Assert
        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public void Delete_Should_Respond_Deleted()
    {
        // Arrange
        TagRepository tagRepo = new TagRepository();


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

        tagRepo.tags.Add(son1);
        tagRepo.tags.Add(son2);
        tagRepo.tags.Add(son3);
        tagRepo.tags.Add(son4);

        // Act
        var actual = tagRepo.Delete(1);
    
        // Assert
        Assert.Equal(Response.Deleted, actual);
    }
    
}
