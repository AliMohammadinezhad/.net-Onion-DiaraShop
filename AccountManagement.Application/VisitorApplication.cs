using AccountManagement.Application.Contract.Visitor;
using AccountManagement.Domain.VisitorAgg;

namespace AccountManagement.Application;

public class VisitorApplication : IVisitorApplication
{
    private readonly IVisitorRepository _visitorRepository;

    public VisitorApplication(IVisitorRepository visitorRepository)
    {
        _visitorRepository = visitorRepository;
    }


    public List<UniqueVisitor> VisitorsList()
    {
        return _visitorRepository.VisitorsList();
    }
}