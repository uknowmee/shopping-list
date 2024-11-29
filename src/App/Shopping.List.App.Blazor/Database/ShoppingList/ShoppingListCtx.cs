using Microsoft.EntityFrameworkCore;

namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class ShoppingListCtx : DbContext
{
    public ShoppingListCtx(DbContextOptions<ShoppingListCtx> options) : base(options)
    {
    }

    public DbSet<ShoppingUser> Users { get; set; }
}