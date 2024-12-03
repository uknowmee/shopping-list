namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class ShoppingUser
{
    public Guid Id { get; } = Guid.Empty;
    public bool IsActive { get; set; } = true;
    public List<ItemList> Lists { get; init; } = [];
    public List<ItemPicture> Pictures { get; init; } = [];


    [Obsolete("Only for EF", true)]
    public ShoppingUser()
    {
    }

    private ShoppingUser(Guid id) => (Id, Lists) = (id, [ItemList.Create(id)]);

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
        var list = Lists.FirstOrDefault(l => l.Id == listId) ?? throw new InvalidOperationException("List not found");
        Lists.Remove(list);
        return list;
    }

    public ItemPicture CreatePictureFromUrl(string url)
    {
        var picture = ItemPicture.Create(Id, url);
        Pictures.Add(picture);
        return picture;
    }
    
    public void ChangePictureForItem(ItemPicture? newPicture, Item item)
    {
        var oldPicture = item.Picture;
        item.Picture = null;

        if (oldPicture is not null &&
            (Lists
                    .SelectMany(l => l.Items)
                    .Where(i => i.Id != item.Id && i.Picture is not null)
                    .ToDictionary(i => i.Id, i => i.Picture?.Id) ?? new Dictionary<Guid, Guid?>()
            ).ContainsValue(oldPicture.Id) is false)
        {
            Pictures.Remove(oldPicture);
        }

        item.Picture = newPicture;
    }
}