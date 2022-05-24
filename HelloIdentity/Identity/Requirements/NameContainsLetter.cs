using Microsoft.AspNetCore.Authorization;

namespace HelloIdentity.Identity.Requirements;

public class NameContainsLetter : IAuthorizationRequirement
{
    public char RequiredLetter { get; set; }
}
