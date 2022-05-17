namespace HelloIdentity.Identity.Entities;

public class User
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUsername { get; set; }
    public string Password { get; set; }
}
