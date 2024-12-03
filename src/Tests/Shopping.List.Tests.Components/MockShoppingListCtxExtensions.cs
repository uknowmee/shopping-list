using Moq;
using Moq.EntityFrameworkCore;
using Shopping.List.App.Blazor.Database.ShoppingList;

namespace Shopping.List.Tests.Components;

public static class MockShoppingListCtxExtensions
{
    public static readonly Guid TestUserId = Guid.Parse("4220b774-c358-42d1-8410-c0fc5e118465");
    public static Guid ListId { get; set; }

    public static Mock<ShoppingListCtx> Setup(this Mock<ShoppingListCtx> mock)
    {
        var db = GetMockDb();
        
        mock.Setup(x => x.Users).ReturnsDbSet(db.AsQueryable());
        mock.Setup(x => x.Pictures).ReturnsDbSet(db.SelectMany(u => u.Pictures).AsQueryable());
        mock.Setup(x => x.Items).ReturnsDbSet(db.SelectMany(u => u.Lists).SelectMany(l => l.Items).AsQueryable());
        mock.Setup(x => x.Lists).ReturnsDbSet(db.SelectMany(u => u.Lists).AsQueryable());
        
        return mock;
    }

    private static List<ShoppingUser> GetMockDb()
    {
        var list = new List<ShoppingUser>();

        var user = ShoppingUser.Create(TestUserId);

        var list1 = user.NewList();
        var list2 = user.NewListAtFront();
        var list3 = user.NewList();

        ListId = list1.Id;

        var item1 = list1.AddItem();
        item1.Category = "Groceries";
        item1.Size = "Medium";
        item1.Quantity = 3;

        var item2 = list1.AddItem();
        item2.Category = "Groceries v2";
        item2.Size = "Small";
        item2.Quantity = 1;

        var item3 = list2.AddItem();
        item3.Category = "Clothing";
        item3.Size = "Large";
        item3.Quantity = 2;

        var item4 = list2.AddItem();
        item4.Category = "clothing 2";
        item4.Size = "Small";
        item4.Quantity = 5;

        var item5 = list3.AddItem();
        item5.Category = "Furniture";
        item5.Size = "Large";
        item5.Quantity = 1;

        var picture1 = user.CreatePictureFromUrl("https://upload.wikimedia.org/wikipedia/commons/3/3f/JPEG_example_flower.jpg");
        user.ChangePictureForItem(picture1, item1);
        user.ChangePictureForItem(picture1, item2);

        var picture3 = user.CreatePictureFromUrl("https://upload.wikimedia.org/wikipedia/commons/c/cc/Yours_Food_Logo.jpg");
        var picture4 = user.CreatePictureFromUrl("https://img.freepik.com/darmowe-wektory/ilustracja-sklad-okragly-fast-food_1284-35627.jpg");
        user.ChangePictureForItem(picture3, item3);
        user.ChangePictureForItem(picture4, item4);

        var picture5 = user.CreatePictureFromUrl("https://www.foodiesfeed.com/wp-content/uploads/2023/06/pouring-honey-on-pancakes.jpg");
        user.ChangePictureForItem(picture5, item5);

        list.Add(user);

        return list;
    }
}