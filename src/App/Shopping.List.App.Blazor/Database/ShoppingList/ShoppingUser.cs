namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class ShoppingUser
{
    public Guid Id { get; } = Guid.Empty;
    public bool IsActive { get; set; } = true;
    public List<ItemList> Lists { get; init; } = [];


    [Obsolete("Only for EF", true)]
    public ShoppingUser()
    {
    }

    private ShoppingUser(Guid id)
    {
        Id = id;
        Lists = [ItemList.Create(id)];
    }

    public static ShoppingUser Create(Guid id) => new ShoppingUser(id);
    
    public ItemList NewList()
    {
        var list = ItemList.Create(Id);
        Lists.Add(list);
        return list;
    }
    
    public ItemList NewListAtFront()
    {
        var list = ItemList.Create(Id);
        Lists.Insert(0, list);
        return list;
    }
    
    public ItemList DeleteList(Guid listId)
    {
        var list = Lists.FirstOrDefault(l => l.Id == listId)
            ?? throw new InvalidOperationException("List not found");
        
        Lists.Remove(list);
        
        return list;
    }
}