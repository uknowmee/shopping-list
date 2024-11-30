using Shopping.List.App.Blazor.Database.ShoppingList;

namespace Shopping.List.Tests.Unit.Model;

public class ItemListTests
{
    private readonly VerifySettings _settings;
    private readonly Guid _userId;

    public ItemListTests()
    {
        _settings = new VerifySettings();

        _settings.UseDirectory("snapshots");
        _settings.ScrubInlineGuids();

        _userId = Guid.NewGuid();
    }

    [Fact]
    public Task CreateItemList_ShouldCreateItemList()
    {
        var itemList = ItemList.Create(_userId);

        return Verify(itemList, _settings);
    }

    [Fact]
    public void CreateItemList_ShouldCreateItemList_WithUserId()
    {
        var itemList = ItemList.Create(_userId);

        Assert.Equal(_userId, itemList.UserId);
    }

    [Fact]
    public void CreateItemList_ShouldCreateItemList_WithItems()
    {
        var itemList = ItemList.Create(_userId);

        Assert.NotEmpty(itemList.Items);
    }

    [Fact]
    public void AddItemToList_ShouldAddItemToList_WithItem()
    {
        var itemList = ItemList.Create(_userId);
        var item = itemList.AddItem();

        Assert.Contains(item, itemList.Items);
    }

    [Fact]
    public Task AddItemToList_ShouldReturnAddedItem()
    {
        var itemList = ItemList.Create(_userId);
        var item = itemList.AddItem();

        return Verify(item, _settings);
    }

    [Fact]
    public void RemoveItemFromList_ShouldRemoveItemFromList_WithItemId()
    {
        var itemList = ItemList.Create(_userId);
        var item = itemList.AddItem();
        var removedItem = itemList.RemoveItem(item.Id);

        Assert.DoesNotContain(removedItem, itemList.Items);
    }

    [Fact]
    public void RemoveItemFromList_ShouldRemoveItemFromList()
    {
        var itemList = ItemList.Create(_userId);
        var item = itemList.AddItem();
        var removedItem = itemList.RemoveItem(item.Id);

        Assert.DoesNotContain(removedItem, itemList.Items);
    }

    [Fact]
    public Task RemoveItemFromList_ShouldThrowInvalidOperationException_WhenItemNotFound()
    {
        var itemList = ItemList.Create(_userId);

        var action = new Func<Item>(() => itemList.RemoveItem(Guid.NewGuid()));

        return Throws(action, _settings);
    }

    [Theory]
    [InlineData("New Name")]
    [InlineData("Another Name")]
    [InlineData("123123")]
    public void NameChange_ShouldChangeName_WithNewName(string name)
    {
        var itemList = ItemList.Create(_userId);
        itemList.Name = name;

        Assert.Equal(name, itemList.Name);
    }
}