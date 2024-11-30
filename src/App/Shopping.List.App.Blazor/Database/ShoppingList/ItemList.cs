using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class ItemList
{
    private DateTimeOffset? _dueTo;
    private bool _isRealized;

    public Guid Id { get; } = Guid.NewGuid();
    public Guid UserId { get; } = Guid.Empty;
    [MaxLength(100)] public string Name { get; set; } = "New Shopping List";
    public DateTimeOffset CreationDate { get; } = DateTimeOffset.UtcNow;

    public bool IsRealized
    {
        get => _isRealized;
        set => _isRealized = SetIsRealized(value);
    }

    public DateTimeOffset? DueTo
    {
        get => _dueTo;
        set => _dueTo = SetDueTo(value ?? throw new InvalidOperationException("DueTo cannot be null"));
    }

    public List<Item> Items { get; set; } = [];

    [NotMapped]
    public DateTime? LocalDueTo
    {
        get => DueTo?.LocalDateTime;
        set => DueTo = value?.ToUniversalTime();
    }

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

    private DateTimeOffset SetDueTo(DateTimeOffset value)
    {
        if (value < DateTimeOffset.UtcNow)
        {
            throw new InvalidOperationException("DueTo cannot be in the past");
        }
        
        if (_isRealized)
        {
            throw new InvalidOperationException("List is already realized");
        }

        return value;
    }

    private bool SetIsRealized(bool value)
    {
        if (_isRealized)
        {
            throw new InvalidOperationException("List is already realized");
        }

        return value;
    }
}