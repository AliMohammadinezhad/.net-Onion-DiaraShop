using AccountManagement.Application.Contract.Visitor;
using Framework.Domain;

namespace AccountManagement.Domain.VisitorAgg;

public interface IVisitorRepository : IRepository<long, Visitor>
{
    List<UniqueVisitor> VisitorsList();
}
