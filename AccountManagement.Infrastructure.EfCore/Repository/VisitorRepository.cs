using AccountManagement.Application.Contract.Visitor;
using AccountManagement.Domain.VisitorAgg;
using Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EfCore.Repository;

public class VisitorRepository : RepositoryBase<long, Visitor>, IVisitorRepository
{
    private readonly AccountContext _context;
    public VisitorRepository(AccountContext context) : base(context)
    {
        _context = context;
    }

    public List<UniqueVisitor> VisitorsList()
    {
        return _context.Visitors.Select(x => new UniqueVisitor
        {
            Id = x.Id,
            VisitDate = x.CreationDate,
            VisitorId = x.VisitorId
        }).ToList();
    }
}