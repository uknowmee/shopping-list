using Microsoft.Playwright;

namespace Shopping.List.Tests.E2E;

[TestFixture]
public class LoggedInTests : LoggedInBase
{
    public IPageAssertions Expect(IPage page) => Assertions.Expect(page);

    [Test]
    public async Task YourShoppingLists_AfterLogIn_Visible()
    {
        await Page.GotoAsync("https://shopping-list.uknowmee.com/ShoppingLists/Home");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Shopping Lists", Exact = true }).ClickAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Your Shopping Lists" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task AddNewList_Clicked_ButtonsShouldBeVisible()
    {
        await Page.GotoAsync("https://shopping-list.uknowmee.com/ShoppingLists/Home");
        await Page.GetByRole(AriaRole.Button, new() { Name = "New" }).ClickAsync();
        
        await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Edit" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Items" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Realized" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Delete" })).ToBeVisibleAsync();
    }
}