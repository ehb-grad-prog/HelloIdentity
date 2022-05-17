using System.ComponentModel.DataAnnotations;

namespace HelloIdentity.Identity.Models;

public class SignInModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    public string? RedirectUrl { get; set; } = "/";
}
