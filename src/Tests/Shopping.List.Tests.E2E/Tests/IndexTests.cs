using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Shopping.List.Tests.E2E.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class IndexTests : PageTest
{
    private const string PageUrl = "https://shopping-list.uknowmee.com/";

    [Test]
    public async Task HasTitle()
    {
        await Page.GotoAsync(PageUrl);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Expect(Page).ToHaveTitleAsync(new Regex("Home"));
    }

    [Test]
    public async Task NavigateToShoppingList_WhenNotLoggedIn_LoginShouldBeVisible()
    {
        await Page.GotoAsync("https://shopping-list.uknowmee.com/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByRole(AriaRole.Link, new() { Name = "Shopping Lists", Exact = true }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Log in", Exact = true })).ToBeVisibleAsync();
    }

    [Test]
    public async Task FromLogin_WhenRegisterAsNewClicked_ShouldGoToRegister()
    {
        await Page.GotoAsync("https://shopping-list.uknowmee.com/");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register as a new user" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Register" })).ToBeVisibleAsync();
        await Expect(Page.GetByPlaceholder("name@example.com")).ToBeVisibleAsync();
        await Expect(Page.Locator("input[name=\"Input\\.Password\"]")).ToBeVisibleAsync();
        await Expect(Page.Locator("input[name=\"Input\\.ConfirmPassword\"]")).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Register" })).ToBeVisibleAsync();
    }
}