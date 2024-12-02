using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blazored.Toast;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Shopping.List.App.Blazor.Components.ShoppingLists.Pages;

namespace Shopping.List.Tests.Components.Pages;

public class ItemsTests : TestContext
{
    public ItemsTests()
    {
        Services.UseServiceProviderFactory(new AutofacServiceProviderFactory(b => b.RegisterModule<TestModule>()));

        this.AddFakePersistentComponentState();
        
        Services.AddBootstrapBlazor();
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.SetupModule("./_content/BootstrapBlazor/Components/Input/BootstrapInput.razor.js");
        JSInterop.SetupVoid("selectAllByFocus", _ => true);
        
        var navigationManager = Services.GetRequiredService<NavigationManager>();
        var uri = navigationManager.GetUriWithQueryParameter("id", TestModule.ListId);
        navigationManager.NavigateTo(uri);
    }

    [Fact]
    public void ShouldRenderButtonBack()
    {
        var cut = RenderComponent<Items>(parameters => parameters.AddCascadingValue(TestModule.MockAuthState()));

        var buttons = cut.FindAll("button");

        buttons[0].MarkupMatches("<button class=\"btn btn-primary\">Back</button>");
    }
}