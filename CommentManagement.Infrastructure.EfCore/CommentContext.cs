using CommentManagement.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore;

namespace CommentManagement.Infrastructure.EfCore;

public class CommentContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }
    public CommentContext(DbContextOptions<CommentContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(CommentContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}