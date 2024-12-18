using Framework.Application;

namespace AccountManagement.Application.Contract.Visitor;

public interface IVisitorApplication
{
    List<UniqueVisitor> VisitorsList();
}