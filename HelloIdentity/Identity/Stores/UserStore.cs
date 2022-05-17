using HelloIdentity.Data;
using HelloIdentity.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace HelloIdentity.Identity.Stores;

public class UserStore : IUserStore<User>, IUserPasswordStore<User>
{
    private readonly HelloIdentityContext _context;

    public UserStore(HelloIdentityContext context)
    {
        _context = context;
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }
        catch (Exception exception)
        {
            return IdentityResult.Failed(new IdentityError() { Description = exception.Message });
        }
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }
        catch (Exception exception)
        {
            return IdentityResult.Failed(new IdentityError() { Description = exception.Message });
        }
    }

    public void Dispose()
    {
        //
    }

    /// <inheritdoc cref="IUserStore{TUser}.FindByIdAsync(string, CancellationToken)"/>
    public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        if (long.TryParse(userId, out long id))
        {
            var user = _context.Users.Where(user => user.Id == id).FirstOrDefault();

            return Task.FromResult(user!);
        }

        return Task.FromResult<User>(null!);
    }

    public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var user = _context.Users
            .Where(user => user.NormalizedUsername.Equals(normalizedUserName))
            .FirstOrDefault()!;

        return Task.FromResult(user!);
    }

    public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUsername);
    }

    public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Password);
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(string.IsNullOrWhiteSpace(user.Password) is false);
    }

    public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUsername = normalizedName;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
    {
        user.Password = passwordHash;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }
        catch (Exception exception)
        {
            return IdentityResult.Failed(new IdentityError() { Description = exception.Message });
        }
    }
}
