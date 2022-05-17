using HelloIdentity.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelloIdentity.Data;

public class HelloIdentityContext : IdentityDbContext<User, Role, long>
{
    public HelloIdentityContext(DbContextOptions<HelloIdentityContext> options) : base(options)
    {
        //
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
