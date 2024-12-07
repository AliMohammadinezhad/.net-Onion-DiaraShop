using AccountManagement.Application.Contract.Role;
using AccountManagement.Domain.RoleAgg;
using Framework.Application;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EfCore.Repository;

public class RoleRepository : RepositoryBase<long, Role>, IRoleRepository
{
    private readonly AccountContext _context;
    public RoleRepository(AccountContext context) : base(context)
    {
        _context = context;
    }

    public EditRole GetDetails(long id)
    {
        return _context.Roles.Select(x => new EditRole()
        {
            Id = x.Id,
            Name = x.Name,
            MappedPermission = MapPermissions(x.Permissions)
        }).AsNoTracking().FirstOrDefault(x => x.Id == id);
    }

    private static List<PermissionDTO> MapPermissions(List<Permission> permissions)
    {
        return permissions.Select(x => new PermissionDTO(x.Code, x.Name)).ToList();
    }

    public List<RoleViewModel> List()
    {
        return _context.Roles.Select(x => new RoleViewModel()
        {
            Id = x.Id,
            CreationDate = x.CreationDate.ToFarsi(),
            Name = x.Name,
        }).ToList();
    }
}