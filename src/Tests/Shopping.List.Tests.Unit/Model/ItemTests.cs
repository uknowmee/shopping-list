using Shopping.List.App.Blazor.Database.ShoppingList;

namespace Shopping.List.Tests.Unit.Model;

public class ItemTests
{
    private readonly VerifySettings _settings;
    private readonly Guid _userId;
    private readonly Guid _itemListId;

    public ItemTests()
    {
        _settings = new VerifySettings();

        _settings.UseDirectory("snapshots");
        _settings.ScrubInlineGuids();

        _userId = Guid.NewGuid();
        _itemListId = Guid.NewGuid();
    }
    
    [Fact]
    public Task CreateItem_ShouldCreateItem()
    {
        var item = Item.Create(_userId, _itemListId);

        return Verify(item, _settings);
    }
    
    [Fact]
    public void CreateItem_ShouldCreateItem_WithUserId()
    {
        var item = Item.Create(_userId, _itemListId);

        Assert.Equal(_userId, item.UserId);
    }
    
    [Fact]
    public void CreateItem_ShouldCreateItem_WithListId()
    {
        var item = Item.Create(_userId, _itemListId);

        Assert.Equal(_itemListId, item.ItemListId);
    }
    
    [Fact]
    public void CreateItem_ShouldCreateItem_WithQuantity()
    {
        var item = Item.Create(_userId, _itemListId);

        Assert.Equal(1, item.Quantity);
    }
    
    [Fact]
    public void ChangeQuantity_ShouldChangeQuantity_WithNewQuantity()
    {
        var item = Item.Create(_userId, _itemListId);
        item.Quantity = 5;

        Assert.Equal(5, item.Quantity);
    }
    
    [Fact]
    public void ChangeQuantity_ShouldNotChangeQuantity_WithZeroQuantity()
    {
        var item = Item.Create(_userId, _itemListId);
        item.Quantity = 0;

        Assert.Equal(1, item.Quantity);
    }
    
    [Fact]
    public void ChangeQuantity_ShouldNotChangeQuantity_WithNegativeQuantity()
    {
        var item = Item.Create(_userId, _itemListId);
        item.Quantity = -5;

        Assert.Equal(1, item.Quantity);
    }
    
    [Fact]
    public Task AddPictureToItem_ShouldAddPictureToItem_WithPicture()
    {
        var item = Item.Create(_userId, _itemListId);
        var picture = ItemPicture.Create(_userId, "img.png");

        item.Picture = picture;

        return Verify(item, _settings);
    }
    
    [Fact]
    public void RemovePictureFromItem_ShouldRemovePictureFromItem()
    {
        var item = Item.Create(_userId, _itemListId);
        var picture = ItemPicture.Create(_userId, "img.png");

        item.Picture = picture;
        item.Picture = null;

        Assert.Null(item.Picture);
    }
    
    [Fact]
    public void MarkItemAsBought_ShouldMarkItemAsBought()
    {
        var item = Item.Create(_userId, _itemListId);
        item.IsBought = true;

        Assert.True(item.IsBought);
    }
    
    [Fact]
    public void MarkItemAsBought_ShouldThrowException_WhenItemIsAlreadyBought()
    {
        var item = Item.Create(_userId, _itemListId);
        item.IsBought = true;

        Assert.Throws<InvalidOperationException>(() => item.IsBought = true);
    }
}