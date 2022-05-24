using HelloIdentity.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HelloIdentity.Identity.Requirements.Handlers;

public class CanDrinkAlcoholRequirementHandler : AuthorizationHandler<CanDrinkAlcoholRequirement>
{
    public UserManager<User> UserManager { get; }

    public CanDrinkAlcoholRequirementHandler(UserManager<User> userManager)
    {
        UserManager = userManager;
    }


    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CanDrinkAlcoholRequirement requirement)
    {
        var user = await UserManager.GetUserAsync(context.User);
        if (user is null)
        {
            context.Fail();
            return;
        }

        if (user.Age >= requirement.MinimumAge)
            context.Succeed(requirement);
        else
            context.Fail();
    }
}
