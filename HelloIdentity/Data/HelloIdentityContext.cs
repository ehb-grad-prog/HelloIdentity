using HelloIdentity.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloIdentity.Data;

public class HelloIdentityContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public HelloIdentityContext(DbContextOptions<HelloIdentityContext> options) : base(options)
    {
        //
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                UserName = "wannes.gennar@ehb.be",
                NormalizedUsername = "WANNES.GENNAR@EHB.BE",
                Password = "AQAAAAEAACcQAAAAEEKDqi4i36ZCbKbMY2vu/Yr5EFoo+Y9o+mk5vctKeeol3rrY78oA31i2v/7KCgyGCw==" // Password123
            }
        );
    }
}
