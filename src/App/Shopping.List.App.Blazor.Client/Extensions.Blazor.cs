using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shopping.List.App.Blazor.Client.User;

namespace Shopping.List.App.Blazor.Client;

public static partial class Extensions
{
    public static WebAssemblyHostBuilder AddBlazor(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

        return builder;
    }

    public static WebAssemblyHost UseBlazor(this WebAssemblyHost app)
    {
        return app;
    }
}