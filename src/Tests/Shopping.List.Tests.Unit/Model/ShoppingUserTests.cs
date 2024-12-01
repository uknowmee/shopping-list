using Shopping.List.App.Blazor.Database.ShoppingList;

namespace Shopping.List.Tests.Unit.Model;

public class ShoppingUserTests
{
    private readonly VerifySettings _settings;

    public ShoppingUserTests()
    {
        _settings = new VerifySettings();

        _settings.UseDirectory("snapshots");
        _settings.ScrubInlineGuids();
    }

    [Fact]
    public Task CreateShoppingUser_ShouldCreateShoppingUser_WithId()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());

        return Verify(shoppingUser, _settings);
    }
    
    [Fact]
    public void CreateNewListForShoppingUser_ShouldCreateNewList_WithUserId()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());
        var list = shoppingUser.NewList();
        var list2 = shoppingUser.NewList();

        Assert.Equal(shoppingUser.Id, list.UserId);
    }
    
    [Fact]
    public void CreateNewListForShoppingUser_ShouldAddNewListToLists()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());
        var list = shoppingUser.NewList();

        Assert.Contains(list, shoppingUser.Lists.ToList());
    }
    
    [Fact]
    public void DeleteListForShoppingUser_ShouldDeleteList_WithListId()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());
        var list = shoppingUser.NewList();
        var deletedList = shoppingUser.DeleteList(list.Id);

        Assert.Equal(list.Id, deletedList.Id);
    }
    
    [Fact]
    public void DeleteListForShoppingUser_ShouldRemoveListFromLists()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());
        var list = shoppingUser.NewList();
        var deletedList = shoppingUser.DeleteList(list.Id);

        Assert.DoesNotContain(deletedList, shoppingUser.Lists);
    }
    
    [Fact]
    public Task DeleteListForShoppingUser_ShouldThrowInvalidOperationException_WhenListNotFound()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());

        var action = new Func<ItemList>(() => shoppingUser.DeleteList(Guid.NewGuid()));
        
        return Throws(action, _settings);
    }
    
    [Fact]
    public void CreateNewListAtFrontForShoppingUser_ShouldAddListAtFrontOfLists()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());
        var list = shoppingUser.NewList();
        var list2 = shoppingUser.NewList();
        var list3 = shoppingUser.NewListAtFront();

        Assert.Equal(list3, shoppingUser.Lists.First());
    }
    
    [Fact]
    public void CreatePictureFromUrl_ShouldCreatePicture_WithUrl()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());
        var picture = shoppingUser.CreatePictureFromUrl("https://example.com/image.png");

        Assert.Equal("https://example.com/image.png", picture.PictureName);
    }
    
    [Fact]
    public void ChangePictureForItem_ShouldChangePictureForItem()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());
        var shoppingList = shoppingUser.Lists.First();
        var item = shoppingList.AddItem();
        var picture = shoppingUser.CreatePictureFromUrl("https://example.com/image.png");
        
        shoppingUser.ChangePictureForItem(picture, item);

        Assert.Equal(picture, item.Picture);
    }
    
    [Fact]
    public void ChangePictureForItem_ShouldRemoveOldPictureFromPictures_WhenNoOtherItemUsesIt()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());
        var shoppingList = shoppingUser.Lists.First();
        var item = shoppingList.AddItem();
        var picture = shoppingUser.CreatePictureFromUrl("https://example.com/image.png");
        var picture2 = shoppingUser.CreatePictureFromUrl("https://example.com/image2.png");
        
        shoppingUser.ChangePictureForItem(picture, item);
        shoppingUser.ChangePictureForItem(picture2, item);

        Assert.DoesNotContain(picture, shoppingUser.Pictures);
    }
    
    [Fact]
    public void ChangePictureForItem_ShouldNotRemoveOldPictureFromPictures_WhenOtherItemUsesIt()
    {
        var shoppingUser = ShoppingUser.Create(Guid.NewGuid());
        var shoppingList = shoppingUser.Lists.First();
        var item = shoppingList.AddItem();
        var item2 = shoppingList.AddItem();
        var picture = shoppingUser.CreatePictureFromUrl("https://example.com/image.png");
        
        shoppingUser.ChangePictureForItem(picture, item);
        shoppingUser.ChangePictureForItem(picture, item2);

        Assert.Contains(picture, shoppingUser.Pictures.ToList());
    }
}