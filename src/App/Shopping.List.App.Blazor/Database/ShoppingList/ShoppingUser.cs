namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class ShoppingUser
{
    public Guid Id { get; set; } = Guid.Empty;
    public bool IsActive { get; set; } = true;
}