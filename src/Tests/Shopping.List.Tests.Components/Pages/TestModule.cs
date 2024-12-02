using System.Security.Claims;
using Autofac;
using Blazored.Toast.Services;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using Shopping.List.App.Blazor.Components.ShoppingLists.Pages;
using Shopping.List.App.Blazor.Database.Auth;
using Shopping.List.App.Blazor.Database.ShoppingList;

namespace Shopping.List.Tests.Components.Pages;

public class TestModule : Module
{
    public static readonly Guid TestUserId = Guid.Parse("4220b774-c358-42d1-8410-c0fc5e118465");
    public static Guid ListId { get; set; }
    
    protected override void Load(ContainerBuilder builder)
    {
        var mockDb = CreateMockShoppingListCtx();
        var userManager = CreateMockUserManager();
        var mockEnvironment = GetMockWebEnv();
        var navigationManager = new FakeNavigationManager(new TestContext());
        var toastService = new Mock<IToastService>();
        var logger1 = new Mock<ILogger<Home>>();
        var logger2 = new Mock<ILogger<Items>>();
        
        builder.RegisterInstance(mockDb.Object).As<ShoppingListCtx>();
        builder.RegisterInstance(toastService.Object).As<IToastService>();
        builder.RegisterInstance(userManager.Object).As<UserManager<ApplicationUser>>();
        builder.RegisterInstance(navigationManager).As<NavigationManager>();
        builder.RegisterInstance(logger1.Object).As<ILogger<Home>>();
        builder.RegisterInstance(logger2.Object).As<ILogger<Items>>();
        builder.RegisterInstance(mockEnvironment.Object).As<IWebHostEnvironment>();
    }

    private static Mock<IWebHostEnvironment> GetMockWebEnv()
    {
        var mock = new Mock<IWebHostEnvironment>();
        mock.Setup(env => env.EnvironmentName).Returns("Development");
        return mock;
    }
    
    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

        var mock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<ApplicationUser>>(),
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<ApplicationUser>>>()
        );

        mock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((ClaimsPrincipal user) => user.Identity?.Name == "TestUser"
                ? new ApplicationUser { UserName = "TestUser", Id = TestUserId }
                : null
            );

        return mock;
    }

    public static Task<AuthenticationState> MockAuthState()
    {
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, "TestUser")], "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(principal));
    }
    
    private static Mock<ShoppingListCtx> CreateMockShoppingListCtx()
    {
        var mock = new Mock<ShoppingListCtx>();
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