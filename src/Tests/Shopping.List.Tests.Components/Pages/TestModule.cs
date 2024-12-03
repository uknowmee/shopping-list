using System.Security.Claims;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blazored.Toast.Services;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shopping.List.App.Blazor.Database.Auth;
using Shopping.List.App.Blazor.Database.ShoppingList;

namespace Shopping.List.Tests.Components;

public class TestModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var mockDb = new Mock<ShoppingListCtx>().Setup();
        var userManager = CreateMockUserManager();
        var mockEnvironment = GetMockWebEnv();
        var navigationManager = new FakeNavigationManager(new TestContext());
        var toastService = new Mock<IToastService>();
        var loggerFactory = new Mock<ILoggerFactory>();

        builder.RegisterInstance(mockDb.Object).As<ShoppingListCtx>();
        builder.RegisterInstance(toastService.Object).As<IToastService>();
        builder.RegisterInstance(userManager.Object).As<UserManager<ApplicationUser>>();
        builder.RegisterInstance(navigationManager).As<NavigationManager>();
        builder.RegisterInstance(loggerFactory.Object).As<ILoggerFactory>();
        builder.RegisterInstance(mockEnvironment.Object).As<IWebHostEnvironment>();

        builder.Populate(new ServiceCollection().AddBootstrapBlazor());
    }

    private static Mock<IWebHostEnvironment> GetMockWebEnv()
    {
        var mock = new Mock<IWebHostEnvironment>();
        mock.Setup(env => env.EnvironmentName).Returns("Development");
        return mock;
    }

    private static Mock<UserManager<ApplicationUser>> CreateMockUserManager()
    {
        var mock = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
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
                ? new ApplicationUser { UserName = "TestUser", Id = MockShoppingListCtxExtensions.TestUserId }
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
}