using Shopping.List.App.Blazor.Database.ShoppingList;

namespace Shopping.List.Tests.Unit.Model;

public class ItemPictureTests
{
    private readonly Guid _userId;
    private readonly Guid _itemListId;

    public ItemPictureTests()
    {
        _userId = Guid.NewGuid();
        _itemListId = Guid.NewGuid();
    }
    
    [Fact]
    public void CreateItemPicture_ShouldCreateItemPicture_WithUserId()
    {
        var itemPicture = ItemPicture.Create(_userId, "name");

        Assert.Equal(_userId, itemPicture.UserId);
    }
    
    [Fact]
    public void CreateItemPicture_ShouldCreateItemPicture_WithPictureName()
    {
        var itemPicture = ItemPicture.Create(_userId, "name");

        Assert.Equal("name", itemPicture.PictureName);
    }
    
    [Fact]
    public void CreateItemPicture_ShouldCreateItemPicture_WithPicturePath()
    {
        var itemPicture = ItemPicture.Create(_userId, "name");

        var separator = Path.DirectorySeparatorChar;
        
        Assert.Equal($"pic{separator}{itemPicture.Id}_{itemPicture.UserId}", itemPicture.PicturePath);
    }
}