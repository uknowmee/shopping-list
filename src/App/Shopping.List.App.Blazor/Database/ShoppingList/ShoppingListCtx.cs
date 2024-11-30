using Microsoft.EntityFrameworkCore;

namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class ShoppingListCtx : DbContext
{
    public ShoppingListCtx(DbContextOptions<ShoppingListCtx> options) : base(options)
    {
    }

    public DbSet<ShoppingUser> Users { get; set; }
    public DbSet<ItemList> Lists { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemPicture> Pictures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoppingUser>(entity =>
            {
                entity.HasMany<ItemList>()
                    .WithOne()
                    .HasForeignKey(l => l.UserId);
            }
        );

        modelBuilder.Entity<Item>(entity =>
            {
                entity.HasOne<ShoppingUser>()
                    .WithMany()
                    .HasForeignKey(i => i.UserId);
                
                entity.HasOne<ItemList>()
                    .WithMany()
                    .HasForeignKey(i => i.ItemListId);
            }
        );
        
        modelBuilder.Entity<ItemPicture>(entity =>
            {
                entity.HasOne<ShoppingUser>()
                    .WithMany()
                    .HasForeignKey(ip => ip.UserId);
            }
        );
    }
}