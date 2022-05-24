using Microsoft.AspNetCore.Identity;

namespace HelloIdentity.Identity.Entities;

public class User : IdentityUser<long>
{
    public int Age { get; set; }
}
