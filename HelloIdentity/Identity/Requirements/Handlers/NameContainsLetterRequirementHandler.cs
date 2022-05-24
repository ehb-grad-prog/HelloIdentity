using HelloIdentity.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HelloIdentity.Identity.Requirements.Handlers;

public class NameContainsLetterRequirementHandler : AuthorizationHandler<NameContainsLetter>
{
    public UserManager<User> UserManager { get; }

    public NameContainsLetterRequirementHandler(UserManager<User> userManager)
    {
        UserManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NameContainsLetter requirement)
    {
        var user = await UserManager.GetUserAsync(context.User);

        if (user is not null)
        {
            if (user.NormalizedUserName.Contains(requirement.RequiredLetter))
            {
                context.Succeed(requirement);
                return;
            }
        }

        context.Fail();
    }
}
