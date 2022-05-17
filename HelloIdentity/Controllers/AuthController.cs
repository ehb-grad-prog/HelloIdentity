using HelloIdentity.Identity.Entities;
using HelloIdentity.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelloIdentity.Controllers;

public class AuthController : Controller
{
    public SignInManager<User> SignInManager { get; }
    public UserManager<User> UserManager { get; }

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        SignInManager = signInManager;
        UserManager = userManager;
    }


    [HttpGet, AllowAnonymous]
    public IActionResult Login(string redirectUrl) => View(new SignInModel()
    {
        RedirectUrl = redirectUrl
    });

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Login(SignInModel model)
    {
        if (ModelState.IsValid is false)
        {
            return View(model);
        }

        var user = await UserManager.FindByNameAsync(model.UserName);
        if (user is null)
        {
            // TODO: login failure message.
            return View(model);
        }

        var result = await SignInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return Redirect(model.RedirectUrl ?? "/");
        }

        // TODO: sign in failure.
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await SignInManager.SignOutAsync();

        return Redirect("/");
    }

    public IActionResult Register() => View();

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Register(SignUpModel model)
    {
        if (ModelState.IsValid is false)
        {
            return View(model);
        }

        var result = await UserManager.CreateAsync(new User
        {
            UserName = model.UserName,
            Email = model.Email
        }, model.Password);

        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Login));
        }

        return View(model);
    }
}
