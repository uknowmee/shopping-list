using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bunit;
using Shopping.List.App.Blazor.Components.ShoppingLists.Pages;

namespace Shopping.List.Tests.Components.Pages;

public class HomeTests : TestContext
{
    public HomeTests()
    {
        Services.UseServiceProviderFactory(new AutofacServiceProviderFactory(b => b.RegisterModule<TestModule>()));
    }

    [Fact]
    public void ShouldRenderButton()
    {
        var cut = RenderComponent<Home>(parameters => parameters.AddCascadingValue(TestModule.MockAuthState()));

        var button = cut.Find("button");

        button.MarkupMatches("<button class=\"btn btn-primary\">New</button>");
    }
}