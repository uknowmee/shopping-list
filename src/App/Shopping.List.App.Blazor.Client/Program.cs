using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shopping.List.App.Blazor.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.AddBlazor().AddCore();

var app = builder.Build();

app.UseBlazor().UseCore();

await app.RunAsync();
