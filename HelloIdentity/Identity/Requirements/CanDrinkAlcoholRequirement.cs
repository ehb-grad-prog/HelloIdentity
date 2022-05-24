using Microsoft.AspNetCore.Authorization;

namespace HelloIdentity.Identity.Requirements;

public class CanDrinkAlcoholRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; set; }
}
