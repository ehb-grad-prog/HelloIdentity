using HelloIdentity.Data;
using HelloIdentity.Identity;
using HelloIdentity.Identity.Entities;
using HelloIdentity.Identity.Requirements;
using HelloIdentity.Identity.Requirements.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HelloIdentityContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlServer(connectionString);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.Approved, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Administrator", "Beheerder", "Gebruiker");
    });

    options.AddPolicy(Policies.NameContainsLetterA, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new NameContainsLetter { RequiredLetter = 'A' });
    });

    options.DefaultPolicy = options.GetPolicy("Approved")!;
});

builder.Services.AddScoped<IAuthorizationHandler, NameContainsLetterRequirementHandler>();
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<HelloIdentityContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

    var users = new User[]
    {
        new User
        {
            UserName = "wannes",
            Email = "wannes.gennar@ehb.be",
            PasswordHash = "Password123!"
        }
    };
    var roles = new Role[]
    {
        new Role
        {
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR"
        },
        new Role
        {
            Name = "Beheerder",
            NormalizedName = "BEHEERDER"
        },
        new Role
        {
            Name = "gebruiker",
            NormalizedName = "GEBRUIKER"
        }
    };
    var claims = new Dictionary<string, Claim[]>
    {
        {
            "Administrator",
            new Claim[]
            {
                new Claim("User", "Manage"),
                new Claim("User", "Read"),
                new Claim("Activity", "Manage"),
                new Claim("Activity", "Read")
            }
        }
    };


    foreach (var user in users)
    {
        if (await userManager.FindByNameAsync(user.UserName) is null)
        {
            var result = await userManager.CreateAsync(user, user.PasswordHash);
            if (result.Succeeded is false)
            {
                throw new Exception($"Failed to add user {user.UserName}");
            }
        }
    }

    foreach (var role in roles)
    {
        if (await roleManager.FindByNameAsync(role.Name) is null)
        {
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded is false)
            {
                throw new Exception($"Failed to add role {role.Name}");
            }

            if (claims.ContainsKey(role.Name))
            {
                var dbRole = await roleManager.FindByNameAsync(role.Name);
                foreach (var claim in claims[role.Name])
                {
                    await roleManager.AddClaimAsync(dbRole, claim);
                }
            }
        }
    }

    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("wannes"), "Administrator");
}

app.Run();
