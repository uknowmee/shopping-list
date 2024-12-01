using System.ComponentModel.DataAnnotations;

namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class Item
{
    private int _quantity = 1;
    private bool _isBought;
    
    public Guid Id { get; } = Guid.NewGuid();
    public Guid UserId { get; } = Guid.Empty;
    public Guid ItemListId { get; } = Guid.Empty;
    public DateTimeOffset CreationDate { get; } = DateTimeOffset.UtcNow;
    public int Quantity { get => _quantity; set => _quantity = value > 0 ? value : 1; }
    [MaxLength(70)] public string Category { get; set; } = "Unknown";
    [MaxLength(50)] public string Size { get; set; } = "Unknown";
    public ItemPicture? Picture { get; set; }
    
    public bool IsBought
    {
        get => _isBought;
        set => _isBought = SetIsRealized(value);
    }

    [Obsolete("Only for EF", true)]
    public Item()
    {
    }
    
    private Item(Guid userId, Guid listId)
    {
        UserId = userId;
        ItemListId = listId;
    }
    
    public static Item Create(Guid userId, Guid listId) => new Item(userId, listId);
    
    private bool SetIsRealized(bool value)
    {
        if (_isBought)
        {
            throw new InvalidOperationException("Item is already bought");
        }

        return value;
    }
}