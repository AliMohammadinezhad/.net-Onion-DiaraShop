using AccountManagement.Application.Contract.Role;
using Framework.Domain;

namespace AccountManagement.Domain.RoleAgg;

public interface IRoleRepository : IRepository<long, Role>
{
    EditRole GetDetails(long id);
    List<RoleViewModel> List();
}