using System.ComponentModel.DataAnnotations;

namespace HelloIdentity.Identity.Models;

public class SignUpModel
{
    [Required, MinLength(3)]
    public string UserName { get; set; }
    public string? Email { get; set; }
    [Required, MinLength(5)]
    public string Password { get; set; }
}
