using Microsoft.Extensions.Configuration;

namespace Shopping.List.Tests.E2E;

public sealed class Config
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(Utils.GetProjectRoot(), "config.json"), false, false)
        .AddJsonFile(Path.Combine(Utils.GetProjectRoot(), "config.local.json"), false, false)
        .Build();

    public static Config Instance { get; } = new(
        Utils.Build<Web>(Configuration, "WebPage"),
        Utils.Build<Creds>(Configuration, "Credentials")
    );


    private readonly Web _webPage;
    private readonly Creds _credentials;

    public string StorageStatePath { get; } = Path.Combine(Utils.GetProjectRoot(), "state.json");
    public string Email => _credentials.Email;
    public string Password => _credentials.Password;
    public string BaseAddress => _webPage.BaseAddress;
    public string Login => $"{BaseAddress}{_webPage.Login}";
    public string ShoppingListsHome => $"{BaseAddress}{_webPage.ShoppingListsHome}";

    private record Web(string BaseAddress, string Login, string ShoppingListsHome)
    {
        public Web() : this(string.Empty, string.Empty, string.Empty) { }
    }

    private record Creds(string Email, string Password)
    {
        public Creds() : this(string.Empty, string.Empty) { }
    }

    private Config(Web webPage, Creds credentials) => (_webPage, _credentials) = (webPage, credentials);
}