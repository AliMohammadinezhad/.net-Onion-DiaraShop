using Framework.Infrastructure;

namespace AccountManagement.Application.Contract.Role;

public class EditRole : CreateRole
{
    public long Id { get; set; }
    public List<PermissionDTO> MappedPermission { get; set; }
}