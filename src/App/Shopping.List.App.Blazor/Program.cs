using Shopping.List.App.Blazor;
using Shopping.List.App.Blazor.Database.Auth;
using Shopping.List.App.Blazor.Database.ShoppingList;
using Shopping.List.Framework.Core;

var builder = WebApplication.CreateBuilder(args);

builder.AddFramework(c => c.ApplyConfiguration())
    .AddDatabase<AuthCtx>("DatabaseSettings:Auth")
    .AddDatabase<ShoppingListCtx>("DatabaseSettings:ShoppingList")
    .AddBlazor()
    .AddCoreServices();

var app = builder.Build();

app.UseFramework()
    .UseBlazor()
    .UseCoreServices()
    .CreateDatabase<AuthCtx>()
    .CreateDatabase<ShoppingListCtx>();

app.Run();
