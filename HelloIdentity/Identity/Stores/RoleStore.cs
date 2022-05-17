using HelloIdentity.Data;
using HelloIdentity.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace HelloIdentity.Identity.Stores;

public class RoleStore : IRoleStore<Role>
{
    private readonly HelloIdentityContext _context;

    public RoleStore(HelloIdentityContext context)
    {
        _context = context;
    }

    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        try
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }
        catch (Exception exception)
        {
            return IdentityResult.Failed(new IdentityError() { Description = exception.Message });
        }
    }

    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        try
        {
            _context.Roles.Remove(role);
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

    public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        if (long.TryParse(roleId, out var id))
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(role!);
        }

        return null!;
    }

    public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        var role = _context.Roles.FirstOrDefault(role => role.NormalizedName.Equals(normalizedRoleName));

        return Task.FromResult(role!);
    }

    public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.NormalizedName);
    }

    public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        return Task.FromResult(role.Name);
    }

    public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
    {
        role.NormalizedName = normalizedName;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
    {
        role.Name = roleName;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
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
