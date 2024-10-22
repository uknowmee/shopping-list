using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Shopping.List.App.Blazor.Client;

public static partial class Extensions
{
    public static WebAssemblyHostBuilder AddCore(this WebAssemblyHostBuilder builder)
    {
        return builder;
    }

    public static WebAssemblyHost UseCore(this WebAssemblyHost app)
    {
        return app;
    }
}