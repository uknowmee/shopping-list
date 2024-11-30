using System.ComponentModel.DataAnnotations;

namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class ItemList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = Guid.Empty;
    [MaxLength(100)] public string Name { get; set; } = "New Shopping List";
    public bool IsRealized { get; set; }
    public DateTimeOffset? DueTo { get; set; }
    public List<Item> Items { get; set; } = [];

    [Obsolete("Only for EF", true)]
    public ItemList()
    {
    }

    private ItemList(Guid userId)
    {
        UserId = userId;
        Items = [Item.Create(userId, Id)];
    }

    public static ItemList Create(Guid userId) => new ItemList(userId);

    public Item AddItem()
    {
        var item = Item.Create(UserId, Id);
        Items.Add(item);
        
        return item;
    }

    public Item RemoveItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new InvalidOperationException("Item not found");
        
        Items.Remove(item);
        
        return item;
    }
}