using BlogManagement.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using Query.Contracts.ArticleCategory;

namespace Query.Query;

public class ArticleCategoryQuery : IArticleCategoryQuery
{
    private readonly BlogContext _context;

    public ArticleCategoryQuery(BlogContext context)
    {
        _context = context;
    }

    public List<ArticleCategoryQueryModel> GetArticleCategories()
    {
        return _context.ArticleCategories
            .Include(x => x.Articles)
            .Select(x => new ArticleCategoryQueryModel
            {
                Id = x.Id,
                Slug = x.Slug,
                Picture = x.Picture,
                CanonicalAddress = x.CanonicalAddress,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                Name = x.Name,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ShowOrder = x.ShowOrder,
                ArticleCount = x.Articles.Count,
            }).ToList();
    }
}