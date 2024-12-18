using AccountManagement.Application.Contract.VisitorService;
using AccountManagement.Domain.VisitorAgg;

namespace AccountManagement.Application;

public class VisitorService : IVisitorService
{
    private readonly IVisitorRepository _visitorRepository;

    public VisitorService(IVisitorRepository visitorRepository)
    {
        _visitorRepository = visitorRepository;
    }

    public void TrackVisitor(string visitorId)
    {
        var visitor = new Visitor
        {
            VisitorId = visitorId,
            CreationDate = DateTime.UtcNow
        };

        _visitorRepository.Create(visitor);
        _visitorRepository.SaveChanges();
    }
}
