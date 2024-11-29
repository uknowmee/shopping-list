using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Shopping.List.App.Blazor.Components.Account;
using Shopping.List.App.Blazor.Database.Auth;
using _Imports = Shopping.List.App.Blazor.Client._Imports;

namespace Shopping.List.App.Blazor;

public static partial class Extensions
{
    public static WebApplicationBuilder AddBlazor(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

        builder.Services.AddAuthorization(options => { });
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }
        ).AddIdentityCookies();

        builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<Role>()
            .AddEntityFrameworkStores<AuthCtx>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        }
        else
        {
            builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
            builder.Services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(3));
            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>();
        }

        return builder;
    }

    public static WebApplication UseBlazor(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(_Imports).Assembly);

        app.MapAdditionalIdentityEndpoints();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();

        return app;
    }
}