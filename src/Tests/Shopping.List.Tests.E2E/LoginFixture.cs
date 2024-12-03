using Microsoft.Playwright;

namespace Shopping.List.Tests.E2E;

[SetUpFixture]
public class LoginFixture
{
    private readonly Config _cfg = Config.Instance;

    public IPlaywright Playwright { get; set; }
    public IBrowser Browser { get; set; }

    [OneTimeSetUp]
    public async Task ProcessLogin()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync();
        var ctx = await Browser.NewContextAsync();
        var page = await ctx.NewPageAsync();

        await page.GotoAsync(_cfg.Login);

        await page.GetByPlaceholder("name@example.com").FillAsync(_cfg.Email);
        await page.GetByPlaceholder("password").FillAsync(_cfg.Password);
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        await ctx.StorageStateAsync(new() { Path = _cfg.StorageStatePath });

        await page.CloseAsync();
        await Browser.CloseAsync();
    }

    [OneTimeTearDown]
    public void ProcessCleanup() => File.Delete(_cfg.StorageStatePath);
}