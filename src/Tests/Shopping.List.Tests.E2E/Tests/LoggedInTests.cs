using Microsoft.Playwright;

namespace Shopping.List.Tests.E2E.Tests;

[NonParallelizable]
[TestFixture]
public class LoggedInTests : LoggedInBase
{
    [Test]
    public async Task YourShoppingLists_AfterLogIn_Visible()
    {
        await Page.GotoAsync("https://shopping-list.uknowmee.com/ShoppingLists/Home");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByRole(AriaRole.Link, new() { Name = "Shopping Lists", Exact = true }).ClickAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Your Shopping Lists" })).ToBeVisibleAsync();
    }

    [Test]
    public async Task AddNewList_Clicked_ButtonsShouldBeVisible()
    {
        await Page.GotoAsync("https://shopping-list.uknowmee.com/ShoppingLists/Home");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByRole(AriaRole.Button, new() { Name = "New" }).ClickAsync();

        await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Edit" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Items" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Realized" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Delete" })).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
    }

    [Test]
    public async Task AddNewList_Clicked_NameDueToAndRealized_ShouldBeVisible()
    {
        await Page.GotoAsync("https://shopping-list.uknowmee.com/ShoppingLists/Home");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.GetByRole(AriaRole.Button, new() { Name = "New" }).ClickAsync();

        await Assertions.Expect(Page.GetByRole(AriaRole.Cell, new() { Name = "New Shopping List" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Cell, new() { Name = "Not Set" })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByText("Not yet")).ToBeVisibleAsync();

        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
    }
    
    [Test]
    public async Task EditShoppingList_ShouldUpdateDetails()
    {
        await Page.GotoAsync("https://shopping-list.uknowmee.com/ShoppingLists/Home");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await Page.GetByRole(AriaRole.Button, new() { Name = "New" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).ClickAsync();
        await Page.Locator("input[type=\"text\"]").ClickAsync();
        await Page.Locator("input[type=\"text\"]").PressAsync("ControlOrMeta+a");
        await Page.Locator("input[type=\"text\"]").FillAsync("dinner");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Cell, new() { Name = "dinner" })).ToBeVisibleAsync();
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
    }
}