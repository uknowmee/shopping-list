using Microsoft.AspNetCore.Identity;

namespace Shopping.List.App.Blazor.Database.Auth;

public sealed class Role : IdentityRole<Guid>
{
    public Role() : base()
    {
        Id = Guid.NewGuid();
    }

    public Role(string someRole) : base(someRole)
    {
        Id = Guid.NewGuid();
    }
}