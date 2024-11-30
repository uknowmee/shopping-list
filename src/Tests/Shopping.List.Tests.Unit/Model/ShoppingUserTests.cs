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

        Assert.Contains(list, shoppingUser.Lists);
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
}