namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class ShoppingList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
}