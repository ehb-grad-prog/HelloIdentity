using Microsoft.AspNetCore.Authorization;

namespace HelloIdentity.Identity.Requirements;

public class MustBeApproved : IAuthorizationRequirement
{
}
