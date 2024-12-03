using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Shopping.List.App.Blazor.Components.ShoppingLists.Pages;

namespace Shopping.List.Tests.Components.Pages;

public class ItemsTests : TestContext
{
    public ItemsTests()
    {
        Services.UseServiceProviderFactory(new AutofacServiceProviderFactory(b => b.RegisterModule<TestModule>()));
        this.SetupTestContext();
    }

    [Fact]
    public void ShouldRenderButtonBack()
    {
        var navigationManager = Services.GetRequiredService<NavigationManager>();
        navigationManager.NavigateTo($"/ShoppingLists/Items?Id={MockShoppingListCtxExtensions.ListId}");
        var cut = RenderComponent<Items>(parameters => parameters.AddCascadingValue(TestModule.MockAuthState()));

        var buttons = cut.FindAll("button");

        buttons[0].MarkupMatches("<button class=\"btn btn-primary\">Back</button>");
    }
}