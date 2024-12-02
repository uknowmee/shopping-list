using Microsoft.Playwright;

namespace Shopping.List.Tests.E2E;

public class LoggedInBase
{
    public IBrowser Browser { get; set; }
    protected IPage Page { get; set; }

    [SetUp]
    public async Task Setup()
    {
        var playwright = await Playwright.CreateAsync();
        Browser = await playwright.Chromium.LaunchAsync();
        
        var context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                StorageStatePath = Path.Combine(ConfigurationBuildingHelper.GetProjectRoot(), "state.json")
            }
        );
        Page = await context.NewPageAsync();
    }

    [TearDown]
    public async Task Cleanup()
    {
        await Page.CloseAsync();
        await Browser.CloseAsync();
    }
}