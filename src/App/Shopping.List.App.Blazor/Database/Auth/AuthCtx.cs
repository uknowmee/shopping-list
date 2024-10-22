using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Shopping.List.App.Blazor.Database.Auth;

public class AuthCtx : IdentityDbContext<ApplicationUser, Role, Guid>
{
    public AuthCtx(DbContextOptions<AuthCtx> options) : base(options)
    {
    }
}