using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace Shopping.List.Tests.E2E;

[SetUpFixture]
public class LoginFixture
{
    public IPlaywright Playwright { get; set; }
    public IBrowser Browser { get; set; }

    private class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    private const string LoginPage = "https://shopping-list.uknowmee.com/Account/Login";

    [OneTimeSetUp]
    public async Task Login()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(ConfigurationBuildingHelper.GetProjectRoot(), "config.local.json"), false, false)
            .Build();
        
        var credentials = ConfigurationBuildingHelper.Build<Credentials>(configuration, "Credentials");
        
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync();
        var ctx = await Browser.NewContextAsync();
        var page = await ctx.NewPageAsync();

        await page.GotoAsync(LoginPage);

        await page.GetByPlaceholder("name@example.com").FillAsync(credentials.Email);
        await page.GetByPlaceholder("password").FillAsync(credentials.Password);
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        
        await ctx.StorageStateAsync(new()
        {
            Path = Path.Combine(ConfigurationBuildingHelper.GetProjectRoot(), "state.json")
        });
        
        await page.CloseAsync();
        await Browser.CloseAsync();
    }
    
    [OneTimeTearDown]
    public static void AssemblyCleanup()
    {
        File.Delete(Path.Combine(ConfigurationBuildingHelper.GetProjectRoot(), "state.json"));
    }
}