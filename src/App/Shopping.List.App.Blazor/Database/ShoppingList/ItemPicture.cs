using System.ComponentModel.DataAnnotations;

namespace Shopping.List.App.Blazor.Database.ShoppingList;

public class ItemPicture
{
    public Guid Id { get; } = Guid.NewGuid();
    public Guid UserId { get; } = Guid.Empty;
    [MaxLength(200)] public string PictureName { get; set; } = string.Empty;
    [MaxLength(80)] public string PicturePath => Path.Combine("pic", $"{Id}_{UserId}");
    
    [Obsolete("Only for EF", true)]
    public ItemPicture()
    {
    }

    private ItemPicture(Guid userId, string picName) => (UserId, PictureName) = (userId, picName);

    public static ItemPicture Create(Guid userId, string picName) => new ItemPicture(userId, picName);
}