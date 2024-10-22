using Microsoft.AspNetCore.Identity;

namespace Shopping.List.App.Blazor.Database.Auth;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser() : base()
    {
        Id = Guid.NewGuid();
    }
}