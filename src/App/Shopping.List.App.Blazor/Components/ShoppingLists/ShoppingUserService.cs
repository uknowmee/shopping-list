using Microsoft.EntityFrameworkCore;
using Shopping.List.App.Blazor.Database.ShoppingList;

namespace Shopping.List.App.Blazor.Components.ShoppingLists;

public interface IShoppingUserService
{
    Task AddShoppingUser(Guid id);
    Task RemoveShoppingUser(Guid id);
}

public class ShoppingUserService : IShoppingUserService
{
    private readonly ShoppingListCtx _ctx;

    public ShoppingUserService(ShoppingListCtx ctx) => _ctx = ctx;

    public async Task AddShoppingUser(Guid id)
    {
        await _ctx.Users.AddAsync(ShoppingUser.Create(id));
        await _ctx.SaveChangesAsync();
    }

    public async Task RemoveShoppingUser(Guid id)
    {
        var user = await _ctx.Users.SingleOrDefaultAsync(u => u.Id == id)
                   ?? throw new InvalidOperationException("User not found! This should not happen!");

        user.IsActive = false;

        await _ctx.SaveChangesAsync();
    }
}